// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  appName: 'Demo',
  apiBaseUrl: 'https://localhost:5001',
  auth: {
    domain: 'dev-spke9m2i.us.auth0.com',
    clientId: 'ReyDS7xhKCmSlo6vrua1AIGIGmQyQA3e',
    audience: 'https://demo',
    redirectUri: window.location.origin
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
import 'zone.js/plugins/zone-error'; // Included with Angular CLI.
