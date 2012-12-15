using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using bc.flash.resources;

namespace bc.flash.native
{
    public enum AppBlendMode
    {
        AlphaBlend,
        NonPremultiplied,
        Additive,
        Opaque,
    }

    public class BcRenderSupport
    {
        enum BatchMode
        {
            None,
            Sprite,
            BasicEffect,
            CustomEffect,
        }

        private static GraphicsDevice graphicsDevice;
        private static SpriteBatch spriteBatch;
        private static BasicEffect basicEffect;
        private static Effect customEffect;
        private static RasterizerState rasterizerState;
        private static SamplerState samplerState;

        private static BatchMode batchMode = BatchMode.None;
        private static Matrix matrix;
        public static Color drawColor;
        private static BcEffect drawEffect;
        private static AppBlendMode blendMode = AppBlendMode.AlphaBlend;

        private static Stack<Matrix> matrixStack = new Stack<Matrix>();

        private static Color transpColor = new Color(0, 0, 0, 0);
        private static Vector2 zeroVector = new Vector2(0, 0);
        private static Matrix worldMatrix;
        private static Matrix viewMatrix;
        private static Matrix projection;

        private static void BeginSpriteBatch(SpriteBatch sb, AppBlendMode blendMode, Matrix m, BatchMode mode)
        {
            Debug.Assert(mode != BatchMode.None);

            if (mode == BatchMode.Sprite)
            {
                BlendState blendState = toBlendState(blendMode);
                sb.Begin(SpriteSortMode.Immediate, blendState, samplerState, null, rasterizerState, drawEffect == null ? null : drawEffect.Effect, m);
            }
            else if (mode == BatchMode.BasicEffect)
            {
                basicEffect.World = Matrix.Multiply(worldMatrix, m);
                basicEffect.CurrentTechnique.Passes[0].Apply();
            }
            else if (mode == BatchMode.CustomEffect)
            {
                customEffect.Parameters["World"].SetValue(Matrix.Multiply(worldMatrix, m));
                customEffect.Parameters["View"].SetValue(viewMatrix);
                customEffect.Parameters["Projection"].SetValue(projection);
                customEffect.CurrentTechnique.Passes[0].Apply();
            }
            batchMode = mode;
        }

        private static BlendState toBlendState(AppBlendMode mode)
        {
            switch (mode)
            {
                case AppBlendMode.AlphaBlend:
                    return BlendState.AlphaBlend;
                case AppBlendMode.Additive:
                    return BlendState.Additive;
                case AppBlendMode.Opaque:
                    return BlendState.Opaque;
                case AppBlendMode.NonPremultiplied:
                    return BlendState.NonPremultiplied;
                default:
                    throw new NotImplementedException();
            }
        }

        private static SpriteBatch GetSpriteBatch(BatchMode mode)
        {
            if (batchMode != mode)
            {
                EndBatch();
                BeginSpriteBatch(spriteBatch, blendMode, matrix, mode);
            }
            return spriteBatch;
        }

        private static void EndBatch()
        {
            //if (batchMode == BatchMode.BasicEffect)
            //{
            //    basicEffect.World = Matrix.Identity;
            //    basicEffect.CurrentTechnique.Passes[0].Apply();
            //}
            //else 
            if (batchMode == BatchMode.Sprite)
            {
                spriteBatch.End();
            }
            else if (batchMode == BatchMode.CustomEffect)
            {
                customEffect = null;
            }

            batchMode = BatchMode.None;
        }

        public static AppBlendMode BlendMode
        {
            get { return blendMode; }
            set
            {
                if (blendMode != value)
                {
                    EndBatch();
                    blendMode = value;
                }
            }
        }

        public static void SetEffect(BcEffect effect)
        {
            if (drawEffect != effect)
            {
                EndBatch();
                drawEffect = effect;
            }
        }

        public static void SetColor(Color color)
        {
            drawColor = color;
        }

        public static Color GetColor()
        {
            return drawColor;
        }

        public static void SetBlendMode(AppBlendMode mode)
        {
            if (blendMode != mode)
            {
                blendMode = mode;
                EndBatch();
            }
        }

        public static void SetSamplerState(SamplerState state)
        {
            if (state != samplerState)
            {
                samplerState = state;
                EndBatch();
            }
        }

        public static AppBlendMode GetBlendMode()
        {
            return blendMode;
        }

        public static void SetMatrix(Matrix m)
        {
            matrix = m;
            EndBatch();
        }

        public static void Begin(GraphicsDevice gd, int width, int height)
        {
            Debug.Assert(batchMode == BatchMode.None, "Bad batch mode: " + batchMode);

            matrixStack.Clear();
            matrix = Matrix.Identity;

            drawColor = Color.White;
            blendMode = AppBlendMode.AlphaBlend;
            samplerState = SamplerState.LinearClamp;

            if (graphicsDevice != gd)
            {
                graphicsDevice = gd;
                spriteBatch = new SpriteBatch(graphicsDevice);
                basicEffect = new BasicEffect(graphicsDevice);

                worldMatrix = Matrix.Identity;
                viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
                projection = Matrix.CreateOrthographicOffCenter(0.0f, width, height, 0, 1.0f, 1000.0f);

                basicEffect.World = worldMatrix;
                basicEffect.View = viewMatrix;
                basicEffect.Projection = projection;
                basicEffect.VertexColorEnabled = true;

                rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
            }
        }

        public static void End()
        {
            EndBatch();
        }

        public static void PushMatrix()
        {
            matrixStack.Push(matrix);
        }

        public static void PopMatrix()
        {
            EndBatch();
            matrix = matrixStack.Pop();
        }

        public static void SetIdentity()
        {
            EndBatch();
            matrix = Matrix.Identity;
        }

        private static void AddTransform(ref Matrix t)
        {
            EndBatch();
            Matrix oldMatrix = matrix;
            Matrix.Multiply(ref t, ref oldMatrix, out matrix);
        }

        public static void Translate(float tx, float ty)
        {
            Matrix transformMatrix = Matrix.CreateTranslation(tx, ty, 0.0f);
            AddTransform(ref transformMatrix);
        }

        public static void Rotate(float rad)
        {
            Matrix transformMatrix = Matrix.CreateRotationZ(rad);
            AddTransform(ref transformMatrix);            
        }

        public static void Scale(float sx, float sy)
        {
            Matrix transformMatrix = Matrix.CreateScale(sx, sy, 1.0f);
            AddTransform(ref transformMatrix);            
        }

        public static void Transform(ref Matrix m)
        {
            AddTransform(ref m);
        }

        public static void ClearTransform()
        {
            EndBatch();
            matrix = Matrix.Identity;
        }

        public static void DrawString(SpriteFont font, float x, float y, String text)
        {
            GetSpriteBatch(BatchMode.Sprite).DrawString(font, text, new Vector2((float)x, (float)y), drawColor);
        }

        public static void DrawImage(BcTexture2D tex, float x, float y)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex.Texture, new Vector2(x, y), drawColor);
        }

        public static void DrawImage(Texture2D tex, float x, float y, Color color)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), color);
        }

        public static void DrawImage(Texture2D tex, ref Rectangle src, float x, float y)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static void DrawImage(Texture2D tex, ref Rectangle src, float x, float y, Color color)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), src, color);
        }

        public static void DrawRect(float x, float y, float width, float height, Color color)
        {
            GetSpriteBatch(BatchMode.BasicEffect);

            VertexPositionColor[] vertexData = new VertexPositionColor[4];
            vertexData[0] = new VertexPositionColor(new Vector3(x, y, 0), color);
            vertexData[1] = new VertexPositionColor(new Vector3(x + width, y, 0), color);
            vertexData[2] = new VertexPositionColor(new Vector3(x + width, y + height, 0), color);
            vertexData[3] = new VertexPositionColor(new Vector3(x, y + height, 0), color);
            short[] indexData = new short[] { 0, 1, 2, 3, 0 };

            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertexData, 0, 4, indexData, 0, 4);
        }

        public static void DrawLine(float x1, float y1, float x2, float y2, Color color)
        {
            GetSpriteBatch(BatchMode.BasicEffect);

            VertexPositionColor[] vertexData = new VertexPositionColor[2];
            vertexData[0] = new VertexPositionColor(new Vector3(x1, y1, 0), color);
            vertexData[1] = new VertexPositionColor(new Vector3(x2, y2, 0), color);

            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexData, 0, 1);
        }

        public static void FillRect(float x, float y, float width, float height, Color color)
        {
            GetSpriteBatch(BatchMode.BasicEffect);

            VertexPositionColor[] vertexData = new VertexPositionColor[4];
            vertexData[0] = new VertexPositionColor(new Vector3(x, y, 0), color);
            vertexData[1] = new VertexPositionColor(new Vector3(x + width, y, 0), color);
            vertexData[2] = new VertexPositionColor(new Vector3(x, y + height, 0), color);
            vertexData[3] = new VertexPositionColor(new Vector3(x + width, y + height, 0), color);

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexData, 0, 2);
        }

        public static void DrawGeometry<T>(PrimitiveType type, T[] vertexData, int primitivesCount) where T : struct, IVertexType
        {
            BlendState oldState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GetSpriteBatch(BatchMode.BasicEffect);
            GraphicsDevice.DrawUserPrimitives(type, vertexData, 0, primitivesCount);
            GraphicsDevice.BlendState = oldState;
        }

        public static void DrawGeometry<T>(PrimitiveType type, T[] vertexData, int primitivesCount, Effect effect) where T : struct, IVertexType
        {
            customEffect = effect;

            BlendState oldState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GetSpriteBatch(BatchMode.CustomEffect);
            GraphicsDevice.DrawUserPrimitives(type, vertexData, 0, primitivesCount);
            GraphicsDevice.BlendState = oldState;
            customEffect = null;
        }

        public static void Clear(Color color)
        {
            EndBatch();
            graphicsDevice.Clear(color);
        }

        public static GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }
    }
}
