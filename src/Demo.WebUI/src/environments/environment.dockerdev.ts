export const environment = {
  production: false,
  appName: 'Demo',
  apiBaseUrl: 'http://localhost:5500',
  auth: {
    domain: 'dev-spke9m2i.us.auth0.com',
    clientId: 'ReyDS7xhKCmSlo6vrua1AIGIGmQyQA3e',
    audience: 'https://demo',
    redirectUri: window.location.origin
  }
};

import 'zone.js/plugins/zone-error';
