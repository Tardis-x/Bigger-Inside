//
// Created by Taras Leskiv on 29/12/2016.
//

#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {

bool _uiDeviceIsMultitaskingSupported() {
    BOOL result = [[UIDevice currentDevice] isMultitaskingSupported];
    return result;
}

char *_uiDeviceGetName() {
    NSString *result = [[UIDevice currentDevice] name];
    return [GoodiesUtils createCStringFrom:result];
}

char *_uiDeviceGetSystemName() {
    NSString *result = [[UIDevice currentDevice] systemName];
    return [GoodiesUtils createCStringFrom:result];
}

char *_uiDeviceGetSystemVersion() {
    NSString *result = [[UIDevice currentDevice] systemVersion];
    return [GoodiesUtils createCStringFrom:result];
}

char *_uiDeviceGetModel() {
    NSString *result = [[UIDevice currentDevice] model];
    return [GoodiesUtils createCStringFrom:result];
}

char *_uiDeviceGetLocalizedModel() {
    NSString *result = [[UIDevice currentDevice] localizedModel];
    return [GoodiesUtils createCStringFrom:result];
}

int _uiDeviceGetUserInterfaceIdiom() {
    UIUserInterfaceIdiom result = [[UIDevice currentDevice] userInterfaceIdiom];
    return result;
}

char *_uiDeviceGetUUID() {
    NSString *result = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
    return [GoodiesUtils createCStringFrom:result];
}

#pragma mark Battery

void _uiDeviceSetBatteryMonitoringEnabled(bool enabled) {
    [[UIDevice currentDevice] setBatteryMonitoringEnabled:enabled];
}

bool _uiDeviceIsBatteryMonitoringEnabled() {
    BOOL result = [[UIDevice currentDevice] isBatteryMonitoringEnabled];
    return result;
}

float _uiDeviceGetBatteryLevel() {
    float result = [[UIDevice currentDevice] batteryLevel];
    return result;
}

int _uiDeviceGetBatteryState() {
    UIDeviceBatteryState result = [[UIDevice currentDevice] batteryState];
    return result;
}

#pragma mark Proximity sensor

void _uiDeviceSetProximityMonitoringEnabled(bool enabled) {
    [[UIDevice currentDevice] setProximityMonitoringEnabled:enabled];
}

bool _uiDeviceIsProximityMonitoringEnabled() {
    BOOL result = [[UIDevice currentDevice] isProximityMonitoringEnabled];
    return result;
}

bool _uiDeviceProximityState() {
    BOOL result = [[UIDevice currentDevice] proximityState];
    return result;
}

void _uiDevicePlayInputClick() {
    [[UIDevice currentDevice] playInputClick];
}

}
#pragma clang diagnostic pop