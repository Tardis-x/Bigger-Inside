//
//  SelectorsContainer.h
//  Unity-iPhone
//
//  Created by Taras Leskiv on 17/05/2017.
//
//

#import <Foundation/Foundation.h>

@interface GoodiesSelectorsContainer : NSObject

@property (nonatomic, copy) void (^onImageSaved)();

@property (nonatomic, copy) void (^onError)();

- (void)thisImage:(UIImage *)image hasBeenSavedInPhotoAlbumWithError:(NSError *)error usingContextInfo:(void*)ctxInfo;

@end

