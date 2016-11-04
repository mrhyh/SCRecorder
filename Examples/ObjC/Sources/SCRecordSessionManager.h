//
//  SCRecordSessionManager.h
//  SCRecorderExamples
//
//  Created by Simon CORSIN on 15/08/14.
//  视频存储管理类，不过是以会话的形式保存的
//

#import <Foundation/Foundation.h>
#import "SCRecorder.h"

@interface SCRecordSessionManager : NSObject

- (void)saveRecordSession:(SCRecordSession *)recordSession;

- (void)removeRecordSession:(SCRecordSession *)recordSession;

- (BOOL)isSaved:(SCRecordSession *)recordSession;

- (void)removeRecordSessionAtIndex:(NSInteger)index;

- (NSArray *)savedRecordSessions;

+ (SCRecordSessionManager *)sharedInstance;

@end
