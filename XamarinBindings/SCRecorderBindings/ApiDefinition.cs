//
// File : ApiDefinition.cs
//
// Author: Simon CORSIN <simoncorsin@gmail.com>
//
// Copyright (c) 2012 Ever SAS
//
// Using or modifying this source code is strictly reserved to Ever SAS.

using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreMedia;
using MonoTouch.CoreImage;
using MonoTouch.GLKit;

namespace SCorsin {

	[BaseType(typeof(NSObject))]
	interface SCRecordSessionSegment {

		[Export("initWithURL:info:")]
		IntPtr Constructor(NSUrl url, NSDictionary info);

		[Export("url")]
		NSUrl Url { get; }

		[Export("asset")]
		AVAsset Asset { get; }

		[Export("duration")]
		CMTime Duration { get; }

		[Export("thumbnail")]
		UIImage Thumbnail { get; }

		[Export("lastImage")]
		UIImage LastImage { get; }

		[Export("frameRate")]
		float FrameRate { get; }

		[Export("info")]
		NSDictionary Info { get; }

		[Export("deleteFile")]
		void DeleteFile();
		
	}

	[BaseType(typeof(NSObject))]
	interface SCPhotoConfiguration {

		[Export("enabled")]
		bool Enabled { get; set; }

		[Export("options")]
		NSDictionary Options { get; set; }

	}

	[BaseType(typeof(NSObject))]
	interface SCMediaTypeConfiguration {

		[Export("enabled")]
		bool Enabled { get; set; }

		[Export("shouldIgnore")]
		bool ShouldIgnore { get; set; }

		[Export("options")]
		NSDictionary Options { get; set; }

		[Export("bitrate")]
		ulong Bitrate { get; set; }

		[Export("preset")]
		NSString Preset { get; set; }

	}

	[BaseType(typeof(SCMediaTypeConfiguration))]
	interface SCVideoConfiguration {

		[Export("size")]
		SizeF Size { get; set; }

		[Export("affineTransform")]
		CGAffineTransform AffineTransform { get; set; }

		[Export("codec")]
		NSString Codec { get; set; }

		[Export("scalingMode")]
		NSString ScalingMode { get; set; }

		[Export("maxFrameRate")]
		int MaxFrameRate { get; set; }

		[Export("timeScale")]
		float TimeScale { get; set; }

		[Export("sizeAsSquare")]
		bool SizeAsSquare { get; set; }

		[Export("shouldKeepOnlyKeyFrames")]
		bool ShouldKeepOnlyKeyFrames { get; set; }

		[Export("keepInputAffineTransform")]
		bool KeepInputAffineTransform { get; set; }

		[Export("filterGroup")]
		SCFilterGroup FilterGroup { get; set; }

		[Export("composition")]
		AVVideoComposition Composition { get; set; }

		[Export("watermarkImage")]
		UIImage WatermarkImage { get; set; }

		[Export("watermarkFrame")]
		RectangleF WatermarkFrame { get; set; }

		[Export("watermarkAnchorLocation")]
		int WatermarkAnchorLocation { get; set; }

	}

	[BaseType(typeof(SCMediaTypeConfiguration))]
	interface SCAudioConfiguration {

		[Export("sampleRate")]
		int SampleRate { get; set; } 

		[Export("channelsCount")]
		int ChannelsCount { get; set; }

		[Export("format")]
		int Format { get; set; }

		[Export("audioMix")]
		AVAudioMix AudioMix { get; set; }
	}

	public delegate void EndRecordSegmentDelegate(int segmentIndex, NSError errore);
	public delegate void GenericErrorDelegate(NSUrl outputUrl, NSError error);

	[BaseType(typeof(NSObject))]
	interface SCRecordSession {

		[Export("initWithDictionaryRepresentation:")]
		IntPtr Constructor(NSDictionary dictionaryRepresentation);

		[Export("identifier")]
		string Identifier { get; }

		[Export("date")]
		NSDate Date { get; }

		[Export("outputUrl")]
		NSUrl OutputUrl { get; }

		[Export("fileType"), NullAllowed]
		NSString FileType { get; set; }

		[Export("fileExtension"), NullAllowed]
		NSString FileExtension { get; set; }

		[Export("segments")]
		SCRecordSessionSegment[] Segments { get; }

		[Export("duration")]
		CMTime Duration { get; }

		[Export("segmentsDuration")]
		CMTime SegmentsDuration { get; }

		[Export("currentSegmentDuration")]
		CMTime CurrentSegmentDuration { get; }

		[Export("recordSegmentBegan")]
		bool RecordSegmentBegan { get; }

		[Export("beginRecordSegment:")]
		void BeginRecordSegment(out NSError error);

		[Export("endRecordSegment:")]
		void EndRecordSegment([NullAllowed] EndRecordSegmentDelegate completionHandler);

		[Export("removeSegmentAtIndex:deleteFile:")]
		void RemoveSegmentAtIndex(int segmentIndex, bool deleteFile);

		[Export("addSegment:")]
		void AddSegment(NSUrl fileUrl);

		[Export("insertSegment:atIndex:")]
		void InsertSegment(NSUrl fileUrl, int segmentIndex);

		[Export("removeAllSegments")]
		void RemoveAllSegments();

		[Export("mergeSegmentsUsingPreset:completionHandler:")]
		void MergeSegments(NSString exportSessionPreset, [NullAllowed] GenericErrorDelegate completionHandler);

		[Export("cancelSession:")]
		void CancelSession([NullAllowed] Action completionHandler);

		[Export("removeLastSegment")]
		void RemoveLastSegment();

		[Export("deinitialize")]
		void Deinitialize();

		[Export("assetRepresentingSegments")]
		AVAsset AssetRepresentingSegments { get; }

		[Export("dictionaryRepresentation")]
		NSDictionary DictionaryRepresentation { get; }

		[Export("recorder")]
		SCRecorder Recorder { get; }
	}

	[Model, BaseType(typeof(NSObject)), Protocol]
	interface SCRecorderDelegate {

		[Abstract, Export("recorder:didReconfigureVideoInput:"), EventArgs("RecorderDidReconfigureVideoInputDelegate")]
		void DidReconfigureVideoInput(SCRecorder recorder, NSError videoInputError);

		[Abstract, Export("recorder:didReconfigureAudioInput:"), EventArgs("RecorderDidReconfigureAudioInputDelegate")]
		void DidReconfigureAudioInput(SCRecorder recorder, NSError audioInputError);

		[Export("recorder:didChangeFlashMode:error:"), Abstract, EventArgs("RecorderDidChangeFlashModeDelegate")]
		void DidChangeFlashMode(SCRecorder recorder, int flashMode, NSError error);

		[Export("recorderWillStartFocus:"), Abstract, EventArgs("RecorderWillStartFocusDelegate")]
		void WillStartFocus(SCRecorder recorder);

		[Export("recorderDidStartFocus:"), Abstract, EventArgs("RecorderDidStartFocusDelegate")]
		void DidStartFocus(SCRecorder recorder);

		[Export("recorderDidEndFocus:"), Abstract, EventArgs("RecorderDidEndFocusDelegate")]
		void DidEndFocus(SCRecorder recorder);

		[Export("recorder:didInitializeAudioInSession:error:"), Abstract, EventArgs("RecorderDidInitializeAudioInRecordSessionDelegate")]
		void DidInitializeAudioInRecordSession(SCRecorder recorder, SCRecordSession recordSession, NSError error);

		[Export("recorder:didInitializeVideoInSession:error:"), Abstract, EventArgs("RecorderDidInitializeVideoInRecordSessionDelegate")]
		void DidInitializeVideoInRecordSession(SCRecorder recorder, SCRecordSession recordSession, NSError error);

		[Export("recorder:didBeginSegmentInSession:error:"), Abstract, EventArgs("RecorderDidBeginRecordSegmentDelegate")]
		void DidBeginRecordSegment(SCRecorder recorder, SCRecordSession recordSession, NSError error);

		[Export("recorder:didCompleteSegment:inSession:error:"), Abstract, EventArgs("RecorderDidEndRecordSegmentDelegate")]
		void DidEndRecordSegment(SCRecorder recorder, SCRecordSessionSegment segment, SCRecordSession session, NSError error);

		[Export("recorder:didAppendVideoSampleBufferInSession:"), Abstract, EventArgs("RecorderDidAppendVideoSampleBufferDelegate")]
		void DidAppendVideoSampleBuffer(SCRecorder recorder, SCRecordSession recordSession);

		[Export("recorder:didAppendAudioSampleBufferInSession:"), Abstract, EventArgs("RecorderDidAppendAudioSampleBufferDelegate")]
		void DidAppendAudioSampleBuffer(SCRecorder recorder, SCRecordSession recordSession);

		[Export("recorder:didSkipAudioSampleBufferInSession:"), Abstract, EventArgs("RecorderDidSkip")]
		void DidSkipAudioSampleBuffer(SCRecorder recorder, SCRecordSession recordSession);

		[Export("recorder:didSkipVideoSampleBufferInSession:"), Abstract, EventArgs("RecorderDidSkip")]
		void DidSkipVideoSampleBuffer(SCRecorder recorder, SCRecordSession recordSession);

		[Export("recorder:didCompleteSession:"), Abstract, EventArgs("RecorderDidCompleteRecordSessionDelegate")]
		void DidCompleteRecordSession(SCRecorder recorder, SCRecordSession recordSession);

		[Export("createSegmentInfoForRecorder:"), DelegateName("CreateSegmentInfoDelegate"), DefaultValue("null")]
		NSDictionary CreateSegmentInfo(SCRecorder recorder);

	}

	public delegate void CapturePhotoDelegate(NSError error, UIImage image);

	[BaseType(typeof(NSObject), Delegates = new string [] { "Delegate" }, Events = new Type [] { typeof(SCRecorderDelegate) })]
	interface SCRecorder {

		[Export("videoConfiguration")]
		SCVideoConfiguration VideoConfiguration { get; }

		[Export("audioConfiguration")]
		SCAudioConfiguration AudioConfiguration { get; }

		[Export("photoConfiguration")]
		SCPhotoConfiguration PhotoConfiguration { get; }

		[Export("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		SCRecorderDelegate Delegate { get; set; }

		[Export("videoEnabledAndReady")]
		bool VideoEnabledAndReady { get; }

		[Export("audioEnabledAndReady")]
		bool AudioEnabledAndReady { get; }

		[Export("fastRecordMethodEnabled")]
		bool FastRecordMethodEnabled { get; set; }

		[Export("isRecording")]
		bool IsRecording { get; }

		[Export("deviceHasFlash")]
		bool DeviceHasFlash { get; }

		[Export("flashMode")]
		int FlashMode { get; set; }

		[Export("device")]
		AVCaptureDevicePosition Device { get; set; }

		[Export("focusMode")]
		AVCaptureFocusMode FocusMode { get; }

		[Export("photoOutputSettings"), NullAllowed]
		NSDictionary PhotoOutputSettings { get; set; }

		[Export("captureSessionPreset")]
		NSString CaptureSessionPreset { get; set; }

		[Export("captureSession")]
		AVCaptureSession CaptureSession { get; }

		[Export("isPrepared")]
		bool IsPrepared { get; }

		[Export("previewLayer")]
		AVCaptureVideoPreviewLayer PreviewLayer { get; }

		[Export("previewView"), NullAllowed]
		UIView PreviewView { get; set; }

		[Export("session"), NullAllowed]
		SCRecordSession Session { get; set; }

		[Export("videoOrientation")]
		AVCaptureVideoOrientation VideoOrientation { get; set; }

		[Export("autoSetVideoOrientation")]
		bool AutoSetVideoOrientation { get; set; }

		[Export("initializeSessionLazily")]
		bool InitializeSessionLazily { get; set; }

		[Export("frameRate")]
		int FrameRate { get; set; }

		[Export("focusSupported")]
		bool FocusSupported { get; }

		[Export("prepare:")]
		bool Prepare(out NSError error);

		[Export("unprepare")]
		void Unprepare();

		[Export("previewViewFrameChanged")]
		void PreviewViewFrameChanged();

		[Export("startRunning")]
		void StartRunning();

		[Export("stopRunning")]
		void StopRunning();

		[Export("beginConfiguration")]
		void BeginConfiguration();

		[Export("commitConfiguration")]
		void CommitConfiguration();

		[Export("switchCaptureDevices")]
		void SwitchCaptureDevices();

		[Export("convertToPointOfInterestFromViewCoordinates:")]
		PointF ConvertToPointOfInterestFromViewCoordinates(PointF viewCoordinates);

		[Export("autoFocusAtPoint:")]
		void AutoFocusAtPoint(PointF point);

		[Export("continuousFocusAtPoint:")]
		void ContinuousFocusAtPoint(PointF point);

		[Export("setActiveFormatWithFrameRate:width:andHeight:error:")]
		bool SetActiveFormatWithFrameRate(int frameRate, int width, int height, out NSError error);

		[Export("focusCenter")]
		void FocusCenter();

		[Export("record")]
		void Record();

		[Export("pause")]
		void Pause();

		[Export("pause:")]
		void Pause(Action completionHandler);

		[Export("capturePhoto:")]
		void CapturePhoto([NullAllowed] CapturePhotoDelegate completionHandler);

		[Export("snapshotOfLastVideoBuffer")]
		UIImage SnapshotOfLastVideoBuffer();

		[Export("CIImageRenderer"), NullAllowed]
		NSObject CIImageRenderer { get; set; }

		[Export("ratioRecorded")]
		float RatioRecorder { get; }

		[Export("maxRecordDuration")]
		CMTime MaxRecordDuration { get; set; }

		[Export("keepMirroringOnWrite")]
		bool KeepMirroringOnWrite { get; set; }

		[Export("error")]
		NSError Error { get; }

	}

	delegate void CompletionHandler(NSError error);

    [BaseType(typeof(NSObject))]
    interface SCAudioTools {

        [Static]
        [Export("overrideCategoryMixWithOthers")]
        void OverrideCategoryMixWithOthers();

		[Static]
		[Export("mixAudio:startTime:withVideo:affineTransform:toUrl:outputFileType:withMaxDuration:withCompletionBlock:")]
		void MixAudioWithVideo(AVAsset audioAsset, CMTime audioStartTime, NSUrl inputUrl, CGAffineTransform affineTransform, NSUrl outputUrl, NSString outputFileType, CMTime maxDuration, CompletionHandler completionHandler);

    }

	[BaseType(typeof(NSObject))]
	interface SCFilter {

		[Export("initWithCIFilter:")]
		IntPtr Constructor(CIFilter filter);

		[Export("coreImageFilter")]
		CIFilter CoreImageFilter { get; }
	}

	[BaseType(typeof(NSObject))]
	interface SCFilterGroup {

		[Export("initWithFilter:")]
		IntPtr Constructor(SCFilter filter);

		[Export("addFilter:")]
		void AddFilter(SCFilter filter);

		[Export("removeFilter:")]
		void RemoveFilter(SCFilter filter);

		[Export("imageByProcessingImage:")]
		CIImage ImageByProcessingImage(CIImage image);

		[Export("filters")]
		SCFilter[] Filters { get; }

		[Export("name")]
		string Name { get; set; }

		[Export("filterGroupWithData:"), Static]
		SCFilterGroup FromData(NSData data);

		[Export("filterGroupWithData:error:"), Static]
		SCFilterGroup FromData(NSData data, out NSError error);

		[Export("filterGroupWithContentsOfUrl:"), Static]
		SCFilterGroup FromUrl(NSUrl url);
	}

	[BaseType(typeof(NSObject))]
	[Model, Protocol]
	interface SCPlayerDelegate {
		[Abstract]	
		[Export("player:didPlay:loopsCount:"), EventArgs("PlayerDidPlay")]
		void DidPlay(SCPlayer player, CMTime currentTime, int loopCount);

		[Abstract]
		[Export("player:didChangeItem:"), EventArgs("PlayerChangedItem")]
        void DidChangeItem(SCPlayer player, [NullAllowed] AVPlayerItem item);

		[Abstract]
		[Export("player:itemReadyToPlay:"), EventArgs("PlayerChangedItem")]
		void ItemReadyToPlay(SCPlayer player, [NullAllowed] AVPlayerItem item);

	}

	[BaseType(typeof(AVPlayer), Delegates = new string [] { "Delegate" }, Events = new Type [] { typeof(SCPlayerDelegate) })]
	interface SCPlayer {

		[Export("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		SCPlayerDelegate Delegate { get; set; }

		[Export("setItemByStringPath:")]
        void SetItem([NullAllowed] string stringPath);

		[Export("setItemByUrl:")]
        void SetItem([NullAllowed] NSUrl url);

		[Export("setItemByAsset:")]
        void SetItem([NullAllowed] AVAsset asset);

		[Export("setItem:")]
        void SetItem([NullAllowed] AVPlayerItem item);

        [Export("setSmoothLoopItemByStringPath:smoothLoopCount:")]
        void SetSmoothLoopItem(string stringPath, uint loopCount);

        [Export("setSmoothLoopItemByUrl:smoothLoopCount:")]
        void SetSmoothLoopItem(NSUrl assetUrl, uint loopCount);

        [Export("setSmoothLoopItemByAsset:smoothLoopCount:")]
        void SetSmoothLoopItem(AVAsset asset, uint loopCount);

		[Export("playableDuration")]
		CMTime PlayableDuration { get; }

		[Export("isPlaying")]
		bool IsPlaying { get; }

		[Export("loopEnabled")]
		bool LoopEnabled { get; set; }

		[Export("beginSendingPlayMessages")]
		void BeginSendingPlayMessages();

		[Export("endSendingPlayMessages")]
		void EndSendingPlayMessages();

		[Export("isSendingPlayMessages")]
		bool IsSendingPlayMessages { get; }

		[Export("autoRotate")]
		bool AutoRotate { get; set; }

		[Export("CIImageRenderer"), NullAllowed]
		NSObject CIImageRenderer { get; set; }

	}

	[BaseType(typeof(UIView))]
	interface SCVideoPlayerView : SCPlayerDelegate {

		[Export("player"), NullAllowed]
		SCPlayer Player { get; set; }

		[Export("playerLayer")]
		AVPlayerLayer PlayerLayer { get; }

		[Export("SCImageViewEnabled")]
		bool SCImageViewEnabled { get; set; }

		[Export("SCImageView")]
		SCImageView SCImageView { get; }
	}

	[BaseType(typeof(UIView))]
	interface SCRecorderToolsView {

		[Export("recorder")]
		SCRecorder Recorder { get; set; }

		[Export("outsideFocusTargetImage")]
		UIImage OutsideFocusTargetImage { get; set; }

		[Export("insideFocusTargetImage")]
		UIImage InsideFocusTargetImage { get; set; }

		[Export("focusTargetSize")]
		SizeF FocusTargetSize { get; set; }

		[Export("showFocusAnimation")]
		void ShowFocusAnimation();

		[Export("hideFocusAnimation")]
		void HideFocusAnimation();

		[Export("minZoomFactor")]
		float MinZoomFactor { get; set; }

		[Export("maxZoomFactor")]
		float MaxZoomFactor { get; set; }

		[Export("tapToFocusEnabled")]
		bool TapToFocusEnabled { get; set; }

		[Export("doubleTapToResetFocusEnabled")]
		bool DoubleTapToResetFocusEnabled { get; set; } 

		[Export("pinchToZoomEnabled")]
		bool PinchToZoomEnabled { get; set; }

		[Export("showsFocusAnimationAutomatically")]
		bool ShowsFocusAnimationAutomatically { get; set; }

	}

	[BaseType(typeof(NSObject))]
	interface SCAssetExportSession {

		[Export("inputAsset")]
		AVAsset InputAsset { get; set; }

		[Export("outputUrl")]
		NSUrl OutputUrl { get; set; }

		[Export("outputFileType")]
		NSString OutputFileType { get; set; }

		[Export("videoConfiguration")]
		SCVideoConfiguration VideoConfiguration { get;}

		[Export("audioConfiguration")]
		SCAudioConfiguration AudioConfiguration { get; }

		[Export("error")]
		NSError Error { get; }

		[Export("initWithAsset:")]
		IntPtr Constructor(AVAsset inputAsset);

		[Export("exportAsynchronouslyWithCompletionHandler:")]
		void ExportAsynchronously(Action completionHandler);

		[Export("useGPUForRenderingFilters")]
		bool UseGPUForRenderingFilters { get; set; }

		[Export("delegate")]
		NSObject Delegate { get; set; }
	}

	[BaseType(typeof(UIView))]
	interface SCFilterSelectorView {

		[Export("filterGroups"), NullAllowed]
		SCFilterGroup[] FilterGroups { get; set; }

		[Export("CIImage"), NullAllowed]
		CIImage CIImage { get; set; }

		[Export("selectedFilterGroup")]
		SCFilterGroup SelectedFilterGroup { get; }

		[Export("preferredCIImageTransform")]
		CGAffineTransform PreferredCIImageTransform { get; set; }

		[Export("currentlyDisplayedImageWithScale:orientation:")]
		UIImage CurrentlyDisplayedImage(float scale, UIImageOrientation orientation);
	}

	[BaseType(typeof(SCFilterSelectorView))]
	interface SCSwipeableFilterView {

		[Export("selectFilterScrollView")]
		UIScrollView SelectFilterScrollView { get; }

		[Export("refreshAutomaticallyWhenScrolling")]
		bool RefreshAutomaticallyWhenScrolling { get; set; }
	}

	[BaseType(typeof(GLKView))]
	interface SCImageView {

		[Export("CIImage"), NullAllowed]
		CIImage CIImage { get; set; }

		[Export("filterGroup"), NullAllowed]
		SCFilterGroup FilterGroup { get; set; }

	}


}
