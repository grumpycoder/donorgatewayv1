//mark.lawrence
//env.js

(function (window) {
    window.__env = window.__env || {};

    switch (window.location.hostname) {
        case 'dg-test':
            window.__env.rsvpUrl = 'rsvp-test';
            break;
        case 'donorgateway.splcenter.org':
            window.__env.rsvpUrl = 'rsvp.splcenter.org';
            break;
        case 'dg':
            window.__env.rsvpUrl = 'rsvp.splcenter.org';
            break;
        case 'donorgateway':
            window.__env.rsvpUrl = 'rsvp.splcenter.org';
            break;
        default:
            window.__env.rsvpUrl = 'localhost:54505';
    }

    // API url
    window.__env.apiUrl = 'http://' + window.location.host + '/api';

    // Base url
    window.__env.baseUrl = '/';

    // Whether or not to enable debug mode
    // Setting this to false will disable console output
    window.__env.enableDebug = true;
}(this));