//
// Created by Taras Leskiv on 15/01/2017.
//

#import <Social/Social.h>
#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
const int TWITTER = 0;
const int FACEBOOK = 1;
const int SINA_WEIBO = 2;
const int TENCENT_WEIBO = 3;
const int LINKED_IN = 4;

#pragma mark Helpers

NSString *
getServiceType(int serviceType) {
    NSString *result;
    switch (serviceType) {
        case TWITTER:
            result = SLServiceTypeTwitter;
            break;
        case FACEBOOK:
            result = SLServiceTypeFacebook;
            break;
        case SINA_WEIBO:
            result = SLServiceTypeSinaWeibo;
            break;
        case TENCENT_WEIBO:
            result = SLServiceTypeTencentWeibo;
            break;
        default:
            break;
    }
    return result;
}

extern "C" {
void _goodiesSocialShare(int serviceType, const char *message, const void *data,
        const unsigned long data_length,
        ActionVoidCallbackDelegate callback,
        void *successPtr, void *cancelPtr) {

    NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
    NSString *serviceTypeStr = getServiceType(serviceType);

    SLComposeViewController *composeViewController = [SLComposeViewController composeViewControllerForServiceType:serviceTypeStr];
    [composeViewController setInitialText:messageStr];

    if (data_length > 0) {
        NSData *imageData = [[NSData alloc] initWithBytes:data length:data_length];
        UIImage *image = [UIImage imageWithData:imageData];
        [composeViewController addImage:image];
    }

    [UnityGetGLViewController() presentViewController:composeViewController
                                             animated:true
                                           completion:nil];

    composeViewController.completionHandler = ^(SLComposeViewControllerResult result) {
        // don't have to hide composeViewController, OS does it automatically
        switch (result) {
            case SLComposeViewControllerResultCancelled: {
                if (cancelPtr) {
                    callback(cancelPtr);
                }
                break;
            }
            case SLComposeViewControllerResultDone: {
                if (successPtr) {
                    callback(successPtr);
                }
                break;
            }
        }
    };
}

bool _isAvailableForServiceType(int serviceType) {
    return [SLComposeViewController isAvailableForServiceType:getServiceType(serviceType)];
}
}

#pragma clang diagnostic pop