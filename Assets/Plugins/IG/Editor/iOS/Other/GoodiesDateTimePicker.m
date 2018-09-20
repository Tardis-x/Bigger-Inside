//
// Created by Taras Leskiv on 24/12/2016.
//

#import "GoodiesDateTimePicker.h"

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(int, IosGoodiesDateTimePickerType) {
    IosGoodiesDateTimePickerTypeTime = 0,
    IosGoodiesDateTimePickerTypeDate = 1,
    IosGoodiesDateTimePickerTypeDateAndTime = 2,
    IosGoodiesDateTimePickerTypeCountDownTimer = 3,
};

@implementation GoodiesDateTimePicker {

}


- (id)initWithCallbackPtr:(void *)callbackPtr
   onDateSelectedDelegate:(OnDateSelectedDelegate *)onDateSelectedDelegate
              onCancelPtr:(void *)onCancelPtr
         onCancelDelegate:(ActionVoidCallbackDelegate *)onCancelDelegate
           datePickerType:(int)datePickerType {
    self = [super init];
    _callbackPtr = callbackPtr;
    _onDateSelectedDelegate = onDateSelectedDelegate;
    _cancelPtr = onCancelPtr;
    _onCancelDelegate = onCancelDelegate;
    _datePickerType = datePickerType;
    return self;
}

- (void)setInitialValuesWithYear:(int)year
                           month:(int)month
                             day:(int)day
                            hour:(int)hour
                          minute:(int)minute {
    NSDateComponents *components = [[NSDateComponents alloc] init];
    [components setYear:year];
    [components setMonth:month];
    [components setDay:day];
    [components setHour:hour];
    [components setMinute:minute];

    NSCalendar *calendar = [NSCalendar currentCalendar];
    _initialDateTime = [calendar dateFromComponents:components];
}

- (void)setInitialValueToNow {
    _initialDateTime = [NSDate new];
}

- (void)showPicker {
    UIView *rootView = UnityGetGLView();

    // Block input behind
    _blockerButton = [[UIButton alloc] initWithFrame:rootView.frame];
    [_blockerButton setBackgroundColor:[UIColor clearColor]];
    _blockerButton.tag = 42;
    _blockerButton.backgroundColor = [[UIColor blackColor] colorWithAlphaComponent:0.6];
    [rootView addSubview:_blockerButton];

    // Date Picker
    _datePicker = [[UIDatePicker alloc] init];
    _datePicker.frame = CGRectMake(0, rootView.frame.size.height - _datePicker.frame.size.height, rootView.frame.size.width, _datePicker.frame.size.height);
    _datePicker.autoresizingMask = UIViewAutoresizingFlexibleTopMargin;

    if (_datePickerType == IosGoodiesDateTimePickerTypeDate) {
        _datePicker.datePickerMode = UIDatePickerModeDate;
    } else if (_datePickerType == IosGoodiesDateTimePickerTypeTime) {
        _datePicker.datePickerMode = UIDatePickerModeTime;
    } else if (_datePickerType == IosGoodiesDateTimePickerTypeDateAndTime) {
        _datePicker.datePickerMode = UIDatePickerModeDateAndTime;
    } else if (_datePickerType == IosGoodiesDateTimePickerTypeCountDownTimer) {
        _datePicker.datePickerMode = UIDatePickerModeCountDownTimer;
    }
    _datePicker.hidden = NO;
    _datePicker.date = _initialDateTime;
    _datePicker.backgroundColor = [UIColor whiteColor];

    //    [_datePicker addTarget:self action:@selector(changeDate:) forControlEvents:UIControlEventValueChanged];

    [UnityGetGLViewController().view addSubview:_datePicker];

    // Toolbar - Done button
    CGRect toolbarRect = CGRectMake(0, _datePicker.frame.origin.y - 44, rootView.frame.size.width, 44);
    _toolbar = [[UIToolbar alloc] initWithFrame:toolbarRect];
    UIBarButtonItem *spacer = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:nil action:nil];
    UIBarButtonItem *doneButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(onDatePicked:)];
    UIBarButtonItem *cancelButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemCancel target:self action:@selector(dismiss:)];
    [_toolbar setItems:@[cancelButton, spacer, doneButton]];

    [UnityGetGLViewController().view addSubview:_toolbar];
}

- (void)dismiss:(id)dismiss {
    NSLog(@"Dismiss");
    [self removeViews];
    _onCancelDelegate(_cancelPtr);
}

- (void)onDatePicked:(id)dismiss {
    [self removeViews];

    NSDateComponents *components = [[NSCalendar currentCalendar] components:NSCalendarUnitDay | NSCalendarUnitMonth | NSCalendarUnitYear | NSCalendarUnitHour | NSCalendarUnitMinute
                                                                   fromDate:_datePicker.date];
    NSInteger day = [components day];
    NSInteger month = [components month];
    NSInteger year = [components year];
    NSInteger hour = [components hour];
    NSInteger minute = [components minute];

    NSLog(@"DateTime: %d %d %d %d %d", (int) year, (int) month, (int) day, (int) hour, (int) minute);

    _onDateSelectedDelegate(_callbackPtr, (int) year, (int) month, (int) day, (int) hour, (int) minute);
}

- (void)removeViews {
    [_blockerButton removeFromSuperview];
    [_toolbar removeFromSuperview];
    [_datePicker removeFromSuperview];
}
@end

NS_ASSUME_NONNULL_END
