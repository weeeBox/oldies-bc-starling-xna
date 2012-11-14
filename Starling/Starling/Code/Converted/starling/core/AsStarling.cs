using System;
 
using bc.flash;
using bc.flash.display;
using bc.flash.display3D;
using bc.flash.errors;
using bc.flash.events;
using bc.flash.geom;
using bc.flash.text;
using bc.flash.ui;
using bc.flash.utils;
using starling.animation;
using starling.core;
using starling.display;
using starling.events;
using AsEvent = bc.flash.events.AsEvent;
using AsKeyboardEvent = bc.flash.events.AsKeyboardEvent;
using AsTouchEvent = bc.flash.events.AsTouchEvent;
using AsStage = starling.display.AsStage;
using AsDisplayObject = starling.display.AsDisplayObject;
using AsSprite = bc.flash.display.AsSprite;
using AsEventDispatcher = starling.events.AsEventDispatcher;
using AsTouchProcessor = starling.core.AsTouchProcessor;
using AsTouchPhase = starling.events.AsTouchPhase;
 
namespace starling.core
{
	public class AsStarling : AsEventDispatcher
	{
		public static String VERSION = "1.2";
		private AsStage3D mStage3D;
		private AsStage mStage;
		private AsClass mRootClass;
		private AsJuggler mJuggler;
		private bool mStarted;
		private AsRenderSupport mSupport;
		private AsTouchProcessor mTouchProcessor;
		private int mAntiAliasing;
		private bool mSimulateMultitouch;
		private bool mEnableErrorChecking;
		private float mLastFrameTimestamp;
		private AsRectangle mViewPort;
		private bool mLeftMouseDown;
		private AsStatsDisplay mStatsDisplay;
		private bool mShareContext;
		private bc.flash.display.AsStage mNativeStage;
		private bc.flash.display.AsSprite mNativeOverlay;
		private AsContext3D mContext;
		private AsDictionary mPrograms;
		private static AsStarling sCurrent;
		private static bool sHandleLostContext;
		public AsStarling(AsClass rootClass, bc.flash.display.AsStage stage, AsRectangle viewPort, AsStage3D stage3D, String renderMode, String profile)
		{
			if((stage == null))
			{
				throw new AsArgumentError("Stage must not be null");
			}
			if((rootClass == null))
			{
				throw new AsArgumentError("Root class must not be null");
			}
			if((viewPort == null))
			{
				viewPort = new AsRectangle(0, 0, stage.getStageWidth(), stage.getStageHeight());
			}
			if((stage3D == null))
			{
				stage3D = stage.getStage3Ds()[0];
			}
			makeCurrent();
			mRootClass = rootClass;
			mViewPort = viewPort;
			mStage3D = stage3D;
			mStage = new AsStage(viewPort.width, viewPort.height, stage.getColor());
			mNativeOverlay = new AsSprite();
			mNativeStage = stage;
			mNativeStage.addChild(mNativeOverlay);
			mTouchProcessor = new AsTouchProcessor(mStage);
			mJuggler = new AsJuggler();
			mAntiAliasing = 0;
			mSimulateMultitouch = false;
			mEnableErrorChecking = false;
			mLastFrameTimestamp = (AsGlobal.getTimer() / 1000.0f);
			mPrograms = new AsDictionary();
			mSupport = new AsRenderSupport();
			AsArray __touchEventTypes_ = getTouchEventTypes();
			if (__touchEventTypes_ != null)
			{
				foreach (String touchEventType in __touchEventTypes_)
				{
					stage.addEventListener(touchEventType, onTouch, false, 0, true);
				}
			}
			stage.addEventListener(AsEvent.ENTER_FRAME, onEnterFrame, false, 0, true);
			stage.addEventListener(AsKeyboardEvent.KEY_DOWN, onKey, false, 0, true);
			stage.addEventListener(AsKeyboardEvent.KEY_UP, onKey, false, 0, true);
			stage.addEventListener(AsEvent.RESIZE, onResize, false, 0, true);
			if(((mStage3D.getContext3D() != null) && (mStage3D.getContext3D().getDriverInfo() != "Disposed")))
			{
				mShareContext = true;
				setTimeout(initialize, 1);
			}
			else
			{
				mShareContext = false;
				mStage3D.addEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated, false, 1, true);
				mStage3D.addEventListener(AsErrorEvent.ERROR, onStage3DError, false, 1, true);
				try
				{
					NOT.IMPLEMENTED();
					{
						mStage3D.requestContext3D(renderMode);
					}
				}
				catch (AsError e)
				{
					showFatalError(("Context3D error: " + e.message));
				}
			}
		}
		public AsStarling(AsClass rootClass, bc.flash.display.AsStage stage, AsRectangle viewPort, AsStage3D stage3D, String renderMode)
		 : this(rootClass, stage, viewPort, stage3D, renderMode, "baselineConstrained")
		{
		}
		public AsStarling(AsClass rootClass, bc.flash.display.AsStage stage, AsRectangle viewPort, AsStage3D stage3D)
		 : this(rootClass, stage, viewPort, stage3D, "auto", "baselineConstrained")
		{
		}
		public AsStarling(AsClass rootClass, bc.flash.display.AsStage stage, AsRectangle viewPort)
		 : this(rootClass, stage, viewPort, null, "auto", "baselineConstrained")
		{
		}
		public AsStarling(AsClass rootClass, bc.flash.display.AsStage stage)
		 : this(rootClass, stage, null, null, "auto", "baselineConstrained")
		{
		}
		public virtual void dispose()
		{
			stop();
			mNativeStage.removeEventListener(AsEvent.ENTER_FRAME, onEnterFrame, false);
			mNativeStage.removeEventListener(AsKeyboardEvent.KEY_DOWN, onKey, false);
			mNativeStage.removeEventListener(AsKeyboardEvent.KEY_UP, onKey, false);
			mNativeStage.removeEventListener(AsEvent.RESIZE, onResize, false);
			mStage3D.removeEventListener(AsEvent.CONTEXT3D_CREATE, onContextCreated, false);
			mStage3D.removeEventListener(AsErrorEvent.ERROR, onStage3DError, false);
			AsArray __touchEventTypes_ = getTouchEventTypes();
			if (__touchEventTypes_ != null)
			{
				foreach (String touchEventType in __touchEventTypes_)
				{
					mNativeStage.removeEventListener(touchEventType, onTouch, false);
				}
			}
			AsDictionary __programs_ = mPrograms;
			if (__programs_ != null)
			{
				foreach (AsProgram3D program in __programs_)
				{
					program.dispose();
				}
			}
			if(((mContext != null) && !(mShareContext)))
			{
				mContext.dispose();
			}
			if(mTouchProcessor != null)
			{
				mTouchProcessor.dispose();
			}
			if(mSupport != null)
			{
				mSupport.dispose();
			}
			if(mStage != null)
			{
				mStage.dispose();
			}
			if((sCurrent == this))
			{
				sCurrent = null;
			}
		}
		private void initialize()
		{
			makeCurrent();
			initializeGraphicsAPI();
			initializeRoot();
			mTouchProcessor.setSimulateMultitouch(mSimulateMultitouch);
			mLastFrameTimestamp = (AsGlobal.getTimer() / 1000.0f);
		}
		private void initializeGraphicsAPI()
		{
			mContext = mStage3D.getContext3D();
			mContext.setEnableErrorChecking(mEnableErrorChecking);
			mPrograms = new AsDictionary();
			updateViewPort();
			AsGlobal.trace("[Starling] Initialization complete.");
			AsGlobal.trace("[Starling] Display Driver:", mContext.getDriverInfo());
			dispatchEventWith(AsEvent.CONTEXT3D_CREATE, false, mContext);
		}
		private void initializeRoot()
		{
			if((mStage.getNumChildren() > 0))
			{
				return;
			}
			NOT.IMPLEMENTED();
		}
		public virtual void nextFrame()
		{
			float now = (AsGlobal.getTimer() / 1000.0f);
			float passedTime = (now - mLastFrameTimestamp);
			mLastFrameTimestamp = now;
			advanceTime(passedTime);
			render();
		}
		public virtual void advanceTime(float passedTime)
		{
			makeCurrent();
			mStage.advanceTime(passedTime);
			mJuggler.advanceTime(passedTime);
			mTouchProcessor.advanceTime(passedTime);
		}
		public virtual void render()
		{
			makeCurrent();
			updateNativeOverlay();
			mSupport.nextFrame();
			if(((mContext == null) || (mContext.getDriverInfo() == "Disposed")))
			{
				return;
			}
			if(!(mShareContext))
			{
				AsRenderSupport.clear(mStage.getColor(), 1.0f);
			}
			mSupport.setOrthographicProjection(mStage.getStageWidth(), mStage.getStageHeight());
			mStage.render(mSupport, 1.0f);
			mSupport.finishQuadBatch();
			if(mStatsDisplay != null)
			{
				mStatsDisplay.setDrawCount(mSupport.getDrawCount());
			}
			if(!(mShareContext))
			{
				mContext.present();
			}
		}
		private void updateViewPort()
		{
			if(mShareContext)
			{
				return;
			}
			if(((mContext != null) && (mContext.getDriverInfo() != "Disposed")))
			{
				mContext.configureBackBuffer((int)(mViewPort.width), (int)(mViewPort.height), mAntiAliasing, false);
			}
			mStage3D.setX(mViewPort.x);
			mStage3D.setY(mViewPort.y);
		}
		private void updateNativeOverlay()
		{
			mNativeOverlay.setX(mViewPort.x);
			mNativeOverlay.setY(mViewPort.y);
			mNativeOverlay.setScaleX((mViewPort.width / mStage.getStageWidth()));
			mNativeOverlay.setScaleY((mViewPort.height / mStage.getStageHeight()));
		}
		private void showFatalError(String message)
		{
			AsTextField textField = new AsTextField();
			AsTextFormat textFormat = new AsTextFormat("Verdana", 12, 0xFFFFFF);
			textFormat.setAlign(AsTextFormatAlign.CENTER);
			textField.setDefaultTextFormat(textFormat);
			textField.setWordWrap(true);
			textField.setWidth((mStage.getStageWidth() * 0.75f));
			textField.setAutoSize(AsTextFieldAutoSize.CENTER);
			textField.setText(message);
			textField.setX(((mStage.getStageWidth() - textField.getWidth()) / 2));
			textField.setY(((mStage.getStageHeight() - textField.getHeight()) / 2));
			textField.setBackground(true);
			textField.setBackgroundColor(0x440000);
			getNativeOverlay().addChild(textField);
		}
		public virtual void makeCurrent()
		{
			sCurrent = this;
		}
		public virtual void start()
		{
			mStarted = true;
			mLastFrameTimestamp = (AsGlobal.getTimer() / 1000.0f);
		}
		public virtual void stop()
		{
			mStarted = false;
		}
		private void onStage3DError(AsErrorEvent _event)
		{
			if((_event.getErrorID() == 3702))
			{
				showFatalError("This application is not correctly embedded (wrong wmode value)");
			}
			else
			{
				showFatalError(("Stage3D error: " + _event.getOwnProperty("text")));
			}
		}
		private void onContextCreated(AsEvent _event)
		{
			if((!(AsStarling.getHandleLostContext()) && (mContext != null)))
			{
				showFatalError("Fatal error: The application lost the device context!");
				stop();
			}
			else
			{
				initialize();
			}
		}
		private void onEnterFrame(AsEvent _event)
		{
			if((mStarted && !(mShareContext)))
			{
				nextFrame();
			}
		}
		private void onKey(AsKeyboardEvent _event)
		{
			if(!(mStarted))
			{
				return;
			}
			makeCurrent();
			mStage.dispatchEvent(new bc.flash.events.AsKeyboardEvent(_event.getType(), _event.getCharCode(), _event.getKeyCode(), _event.getKeyLocation(), _event.getCtrlKey(), _event.getAltKey(), _event.getShiftKey()));
		}
		private void onResize(bc.flash.events.AsEvent _event)
		{
			bc.flash.display.AsStage stage = ((_event.getTarget() is AsStage) ? ((AsStage)(_event.getTarget())) : null);
			mStage.dispatchEvent(new AsResizeEvent(AsEvent.RESIZE, stage.getStageWidth(), stage.getStageHeight()));
		}
		private void onTouch(AsEvent _event)
		{
			if(!(mStarted))
			{
				return;
			}
			float globalX = 0;
			float globalY = 0;
			int touchID = 0;
			String phase = null;
			if(_event is AsMouseEvent)
			{
				AsMouseEvent mouseEvent = ((_event is AsMouseEvent) ? ((AsMouseEvent)(_event)) : null);
				globalX = mouseEvent.getStageX();
				globalY = mouseEvent.getStageY();
				touchID = 0;
				if((_event.getType() == AsMouseEvent.MOUSE_DOWN))
				{
					mLeftMouseDown = true;
				}
				else
				{
					if((_event.getType() == AsMouseEvent.MOUSE_UP))
					{
						mLeftMouseDown = false;
					}
				}
			}
			else
			{
				AsTouchEvent touchEvent = ((_event is AsTouchEvent) ? ((AsTouchEvent)(_event)) : null);
				globalX = touchEvent.getStageX();
				globalY = touchEvent.getStageY();
				touchID = touchEvent.getTouchPointID();
			}
			switch(_event.getType())
			{
				case AsTouchEvent.TOUCH_BEGIN:
				phase = AsTouchPhase.BEGAN;
				break;
				case AsTouchEvent.TOUCH_MOVE:
				phase = AsTouchPhase.MOVED;
				break;
				case AsTouchEvent.TOUCH_END:
				phase = AsTouchPhase.ENDED;
				break;
				case AsMouseEvent.MOUSE_DOWN:
				phase = AsTouchPhase.BEGAN;
				break;
				case AsMouseEvent.MOUSE_UP:
				phase = AsTouchPhase.ENDED;
				break;
				case AsMouseEvent.MOUSE_MOVE:
				phase = ((mLeftMouseDown) ? (AsTouchPhase.MOVED) : (AsTouchPhase.HOVER));
				break;
			}
			globalX = ((mStage.getStageWidth() * (globalX - mViewPort.x)) / mViewPort.width);
			globalY = ((mStage.getStageHeight() * (globalY - mViewPort.y)) / mViewPort.height);
			mTouchProcessor.enqueue(touchID, phase, globalX, globalY);
		}
		private AsArray getTouchEventTypes()
		{
			return (((AsMouse.getSupportsCursor() || !(getMultitouchEnabled()))) ? (new AsArray(AsMouseEvent.MOUSE_DOWN, AsMouseEvent.MOUSE_MOVE, AsMouseEvent.MOUSE_UP)) : (new AsArray(AsTouchEvent.TOUCH_BEGIN, AsTouchEvent.TOUCH_MOVE, AsTouchEvent.TOUCH_END)));
		}
		public virtual void registerProgram(String name, AsByteArray vertexProgram, AsByteArray fragmentProgram)
		{
			if(mPrograms.containsKey(name))
			{
				throw new AsError("Another program with this name is already registered");
			}
			AsProgram3D program = mContext.createProgram();
			program.upload(vertexProgram, fragmentProgram);
			mPrograms[name] = program;
		}
		public virtual void deleteProgram(String name)
		{
			AsProgram3D program = getProgram(name);
			if(program != null)
			{
				program.dispose();
				mPrograms.remove(name);
			}
		}
		public virtual AsProgram3D getProgram(String name)
		{
			return ((mPrograms[name] is AsProgram3D) ? ((AsProgram3D)(mPrograms[name])) : null);
		}
		public virtual bool hasProgram(String name)
		{
			return mPrograms.containsKey(name);
		}
		public virtual bool getIsStarted()
		{
			return mStarted;
		}
		public virtual AsJuggler getJuggler()
		{
			return mJuggler;
		}
		public virtual AsContext3D getContext()
		{
			return mContext;
		}
		public virtual bool getSimulateMultitouch()
		{
			return mSimulateMultitouch;
		}
		public virtual void setSimulateMultitouch(bool _value)
		{
			mSimulateMultitouch = _value;
			if(mContext != null)
			{
				mTouchProcessor.setSimulateMultitouch(_value);
			}
		}
		public virtual bool getEnableErrorChecking()
		{
			return mEnableErrorChecking;
		}
		public virtual void setEnableErrorChecking(bool _value)
		{
			mEnableErrorChecking = _value;
			if(mContext != null)
			{
				mContext.setEnableErrorChecking(_value);
			}
		}
		public virtual int getAntiAliasing()
		{
			return mAntiAliasing;
		}
		public virtual void setAntiAliasing(int _value)
		{
			mAntiAliasing = _value;
			updateViewPort();
		}
		public virtual AsRectangle getViewPort()
		{
			return mViewPort.clone();
		}
		public virtual void setViewPort(AsRectangle _value)
		{
			mViewPort = _value.clone();
			updateViewPort();
		}
		public virtual float getContentScaleFactor()
		{
			return (mViewPort.width / mStage.getStageWidth());
		}
		public virtual AsSprite getNativeOverlay()
		{
			return mNativeOverlay;
		}
		public virtual bool getShowStats()
		{
			return (mStatsDisplay != null);
		}
		public virtual void setShowStats(bool _value)
		{
			if(((mStatsDisplay != null) && !(_value)))
			{
				mStatsDisplay.removeFromParent(true);
				mStatsDisplay = null;
			}
			else
			{
				if(((mStatsDisplay == null) && _value))
				{
					showStatsAt();
				}
			}
		}
		public virtual void showStatsAt(String hAlign, String vAlign)
		{
			NOT.IMPLEMENTED();
		}
		public virtual void showStatsAt(String hAlign)
		{
			showStatsAt(hAlign, "top");
		}
		public virtual void showStatsAt()
		{
			showStatsAt("left", "top");
		}
		public virtual AsStage getStage()
		{
			return mStage;
		}
		public virtual AsStage3D getStage3D()
		{
			return mStage3D;
		}
		public virtual bc.flash.display.AsStage getNativeStage()
		{
			return mNativeStage;
		}
		public virtual AsDisplayObject getRoot()
		{
			return mStage.getChildAt(0);
		}
		public virtual bool getShareContext()
		{
			return mShareContext;
		}
		public virtual void setShareContext(bool _value)
		{
			mShareContext = _value;
		}
		public static AsStarling getCurrent()
		{
			return sCurrent;
		}
		public static AsContext3D getContext()
		{
			return ((sCurrent != null) ? (sCurrent.getContext()) : (null));
		}
		public static AsJuggler getJuggler()
		{
			return ((sCurrent != null) ? (sCurrent.getJuggler()) : (null));
		}
		public static float getContentScaleFactor()
		{
			return ((sCurrent != null) ? (sCurrent.getContentScaleFactor()) : (1.0f));
		}
		public static bool getMultitouchEnabled()
		{
			return (AsMultitouch.getInputMode() == AsMultitouchInputMode.TOUCH_POINT);
		}
		public static void setMultitouchEnabled(bool _value)
		{
			if(sCurrent != null)
			{
				throw new AsIllegalOperationError("'multitouchEnabled' must be set before Starling instance is created");
			}
			else
			{
				AsMultitouch.setInputMode(((_value) ? (AsMultitouchInputMode.TOUCH_POINT) : (AsMultitouchInputMode.NONE)));
			}
		}
		public static bool getHandleLostContext()
		{
			return sHandleLostContext;
		}
		public static void setHandleLostContext(bool _value)
		{
			if(sCurrent != null)
			{
				throw new AsIllegalOperationError("'handleLostContext' must be set before Starling instance is created");
			}
			else
			{
				sHandleLostContext = _value;
			}
		}
	}
}
