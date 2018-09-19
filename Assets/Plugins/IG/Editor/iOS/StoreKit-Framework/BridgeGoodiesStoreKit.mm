//
// Created by Taras Leskiv on 29/12/2016.
//

#import <StoreKit/StoreKit.h>
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {

void _goodiesRequestReview() {
    [SKStoreReviewController requestReview];
}


}
#pragma clang diagnostic pop