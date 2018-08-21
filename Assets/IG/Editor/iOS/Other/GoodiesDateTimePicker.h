//
// Created by Taras Leskiv on 24/12/2016.
//


#import <Foundation/Foundation.h>
#import "BridgeGoodisFunctionDefs.h"

NS_ASSUME_NONNULL_BEGIN

@interface GoodiesDateTimePicker : NSObject {
    void *_callbackPtr;
    OnDateSelectedDelegate *_onDateSelectedDelegate;
    void *_cancelPtr;
    ActionVoidCallbackDelegate *_onCancelDelegate;
    int _datePickerType;
    NSDate *_initialDateTime;

    UIButton *_blockerButton;
    UIDatePicker *_datePicker;
    UIToolbar *_toolbar;
}

- (id)initWithCallbackPtr:(void *)callbackPtr
   onDateSelectedDelegate:(OnDateSelectedDelegate *)onDateSelectedDelegate
              onCancelPtr:(void *)onCancelPtr
         onCancelDelegate:(ActionVoidCallbackDelegate *)onCancelDelegate
           datePickerType:(int)datePickerType;

- (void)setInitialValuesWithYear:(int)year
                           month:(int)month
                             day:(int)day
                            hour:(int)hour
                          minute:(int)minute;

- (void)setInitialValueToNow;

- (void)showPicker;

@end

NS_ASSUME_NONNULL_END
