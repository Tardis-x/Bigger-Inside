//
// Created by Taras Leskiv on 18/01/2017.
//

#import "GoodiesImagePickerDelegate.h"

@implementation GoodiesImagePickerDelegate {
}

- (instancetype)initWithCompressionQuality:(float)compressionQuality {
    self = [super init];
    if (self) {
        _compressionQuality = compressionQuality;
    }

    return self;
}


- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *, id> *)info {
    UIImage *img = [info valueForKey:UIImagePickerControllerEditedImage];
    if (img == nil) {
        img = [info valueForKey:UIImagePickerControllerOriginalImage];
    }

    img = [self normalizeImage:img];

    NSData *pictureData = UIImageJPEGRepresentation(img, _compressionQuality);
    _imagePicked(pictureData.bytes, pictureData.length);
    pictureData = nil;
}

- (UIImage *)normalizeImage:(UIImage *)img {
    if (img.imageOrientation != UIImageOrientationUp) {
        UIGraphicsBeginImageContextWithOptions(img.size, NO, img.scale);
        [img drawInRect:(CGRect) {0, 0, img.size}];
        img = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
    }
    return img;
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    if (_imagePickCancelled) {
        _imagePickCancelled();
    }
}
@end
