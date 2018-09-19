#import "GoodiesDateTimePicker.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
GoodiesDateTimePicker *newPicker(void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
        void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType) {
    return [[GoodiesDateTimePicker alloc] initWithCallbackPtr:callbackPtr
                                       onDateSelectedDelegate:onDateSelectedDelegate
                                                  onCancelPtr:cancelPtr
                                             onCancelDelegate:onCancel
                                               datePickerType:datePickerType];
}

extern "C" {

GoodiesDateTimePicker *pickerController;

void _showDatePickerWithInitialValue(
        int year, int month, int day, int hourOfDay, int minute,
        void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
        void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType) {
    pickerController = nil;
    pickerController = newPicker(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel, datePickerType);
    [pickerController setInitialValuesWithYear:year
                                         month:month day:day hour:hourOfDay minute:minute];
    [pickerController showPicker];
}

void _showDatePicker(
        void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
        void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType) {
    pickerController = nil;
    pickerController = newPicker(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel, datePickerType);
    [pickerController setInitialValueToNow];
    [pickerController showPicker];
}
}


#pragma clang diagnostic pop