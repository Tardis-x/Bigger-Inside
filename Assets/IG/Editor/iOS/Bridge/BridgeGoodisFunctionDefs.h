typedef void(ActionVoidCallbackDelegate)(void *actionPtr);

typedef void(ActionIntCallbackDelegate)(void *actionPtr, int data);

typedef void(ActionStringCallbackDelegate)(void *actionPtr, const char *data);

typedef void(OnDateSelectedDelegate)(void *callbackPtr, int year, int month, int day, int hour, int minute);
