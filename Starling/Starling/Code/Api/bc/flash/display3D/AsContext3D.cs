using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.display3D;
using bc.flash.display3D.textures;
using bc.flash.errors;
using bc.flash.geom;
using bc.flash.utils;
 
namespace bc.flash.display3D
{
	public class AsContext3D : AsObject
	{
		public virtual void clear(float red, float green, float blue, float alpha, float depth, uint stencil, uint mask)
		{
			throw new AsNotImplementedError();
		}
		public virtual void clear(float red, float green, float blue, float alpha, float depth, uint stencil)
		{
			clear(red, green, blue, alpha, depth, stencil, (uint)(0xffffffff));
		}
		public virtual void clear(float red, float green, float blue, float alpha, float depth)
		{
			clear(red, green, blue, alpha, depth, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void clear(float red, float green, float blue, float alpha)
		{
			clear(red, green, blue, alpha, 1.0f, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void clear(float red, float green, float blue)
		{
			clear(red, green, blue, 1.0f, 1.0f, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void clear(float red, float green)
		{
			clear(red, green, 0.0f, 1.0f, 1.0f, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void clear(float red)
		{
			clear(red, 0.0f, 0.0f, 1.0f, 1.0f, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void clear()
		{
			clear(0.0f, 0.0f, 0.0f, 1.0f, 1.0f, (uint)(0), (uint)(0xffffffff));
		}
		public virtual void configureBackBuffer(int width, int height, int antiAlias, bool enableDepthAndStencil)
		{
			throw new AsNotImplementedError();
		}
		public virtual void configureBackBuffer(int width, int height, int antiAlias)
		{
			configureBackBuffer(width, height, antiAlias, true);
		}
		public virtual AsCubeTexture createCubeTexture(int size, String format, bool optimizeForRenderToTexture)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsIndexBuffer3D createIndexBuffer(int numIndices)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsProgram3D createProgram()
		{
			throw new AsNotImplementedError();
		}
		public virtual AsTexture createTexture(int width, int height, String format, bool optimizeForRenderToTexture)
		{
			throw new AsNotImplementedError();
		}
		public virtual AsVertexBuffer3D createVertexBuffer(int numVertices, int data32PerVertex)
		{
			throw new AsNotImplementedError();
		}
		public virtual void dispose()
		{
			throw new AsNotImplementedError();
		}
		public virtual void drawToBitmapData(AsBitmapData destination)
		{
			throw new AsNotImplementedError();
		}
		public virtual void drawTriangles(AsIndexBuffer3D indexBuffer, int firstIndex, int numTriangles)
		{
			throw new AsNotImplementedError();
		}
		public virtual void drawTriangles(AsIndexBuffer3D indexBuffer, int firstIndex)
		{
			drawTriangles(indexBuffer, firstIndex, -1);
		}
		public virtual void drawTriangles(AsIndexBuffer3D indexBuffer)
		{
			drawTriangles(indexBuffer, 0, -1);
		}
		public virtual String getDriverInfo()
		{
			throw new AsNotImplementedError();
		}
		public virtual bool getEnableErrorChecking()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setEnableErrorChecking(bool toggle)
		{
			throw new AsNotImplementedError();
		}
		public virtual void present()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setBlendFactors(String sourceFactor, String destinationFactor)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setColorMask(bool red, bool green, bool blue, bool alpha)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setCulling(String triangleFaceToCull)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setDepthTest(bool depthMask, String passCompareMode)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setProgram(AsProgram3D program)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setProgramConstantsFromByteArray(String programType, int firstRegister, int numRegisters, AsByteArray data, uint byteArrayOffset)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setProgramConstantsFromMatrix(String programType, int firstRegister, AsMatrix3D matrix, bool transposedMatrix)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setProgramConstantsFromMatrix(String programType, int firstRegister, AsMatrix3D matrix)
		{
			setProgramConstantsFromMatrix(programType, firstRegister, matrix, false);
		}
		public virtual void setProgramConstantsFromVector(String programType, int firstRegister, AsVector<float> data, int numRegisters)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setProgramConstantsFromVector(String programType, int firstRegister, AsVector<float> data)
		{
			setProgramConstantsFromVector(programType, firstRegister, data, -1);
		}
		public virtual void setRenderToBackBuffer()
		{
			throw new AsNotImplementedError();
		}
		public virtual void setRenderToTexture(AsTextureBase texture, bool enableDepthAndStencil, int antiAlias, int surfaceSelector)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setRenderToTexture(AsTextureBase texture, bool enableDepthAndStencil, int antiAlias)
		{
			setRenderToTexture(texture, enableDepthAndStencil, antiAlias, 0);
		}
		public virtual void setRenderToTexture(AsTextureBase texture, bool enableDepthAndStencil)
		{
			setRenderToTexture(texture, enableDepthAndStencil, 0, 0);
		}
		public virtual void setRenderToTexture(AsTextureBase texture)
		{
			setRenderToTexture(texture, false, 0, 0);
		}
		public virtual void setScissorRectangle(AsRectangle rectangle)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setStencilActions(String triangleFace, String compareMode, String actionOnBothPass, String actionOnDepthFail, String actionOnDepthPassStencilFail)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setStencilActions(String triangleFace, String compareMode, String actionOnBothPass, String actionOnDepthFail)
		{
			setStencilActions(triangleFace, compareMode, actionOnBothPass, actionOnDepthFail, "keep");
		}
		public virtual void setStencilActions(String triangleFace, String compareMode, String actionOnBothPass)
		{
			setStencilActions(triangleFace, compareMode, actionOnBothPass, "keep", "keep");
		}
		public virtual void setStencilActions(String triangleFace, String compareMode)
		{
			setStencilActions(triangleFace, compareMode, "keep", "keep", "keep");
		}
		public virtual void setStencilActions(String triangleFace)
		{
			setStencilActions(triangleFace, "always", "keep", "keep", "keep");
		}
		public virtual void setStencilActions()
		{
			setStencilActions("frontAndBack", "always", "keep", "keep", "keep");
		}
		public virtual void setStencilReferenceValue(uint referenceValue, uint readMask, uint writeMask)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setStencilReferenceValue(uint referenceValue, uint readMask)
		{
			setStencilReferenceValue(referenceValue, readMask, (uint)(255));
		}
		public virtual void setStencilReferenceValue(uint referenceValue)
		{
			setStencilReferenceValue(referenceValue, (uint)(255), (uint)(255));
		}
		public virtual void setTextureAt(int sampler, AsTextureBase texture)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setVertexBufferAt(int index, AsVertexBuffer3D buffer, int bufferOffset, String format)
		{
			throw new AsNotImplementedError();
		}
		public virtual void setVertexBufferAt(int index, AsVertexBuffer3D buffer, int bufferOffset)
		{
			setVertexBufferAt(index, buffer, bufferOffset, "float4");
		}
		public virtual void setVertexBufferAt(int index, AsVertexBuffer3D buffer)
		{
			setVertexBufferAt(index, buffer, 0, "float4");
		}
	}
}
