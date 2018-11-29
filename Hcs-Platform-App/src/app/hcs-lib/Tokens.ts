import { InjectionToken } from '@angular/core';

export const HCS_FUNCTION_NAME = new InjectionToken<string>('platform function name');

export const HCS_FUNCTION_ROUTE = new InjectionToken<string>('platform function route');

export const HCS_EXPORT_LIMIT = new InjectionToken<number>('export limit');
export const HCS_ENABLE_STATE = new InjectionToken<boolean>('enable State');
