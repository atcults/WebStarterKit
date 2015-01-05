define([], function()
{
    return {
        defaultRoutePath: '/',
        routes: {
            '/': {
                templateUrl: '/home/index.html',
                dependencies: [
                    'controllers/HomeIndex',
                    'directives/app-color'
                ]
            },
            '/about': {
                templateUrl: '/home/about.html',
                dependencies: [
                    'controllers/HomeAbout',
                    'directives/app-color'
                ]
            },
            '/contact': {
                templateUrl: '/home/contact.html',
                dependencies: [
                    'controllers/HomeContact',
                    'directives/app-color'
                ]
            },
            '/account/login': {
                templateUrl: '/account/login.html',
                dependencies: [
                    'directives/app-color'
                ]
            },
            '/account/signup': {
                templateUrl: '/account/signup.html',
                dependencies: [
                    'directives/app-color'
                ]
            },
            '/account/change': {
                templateUrl: '/account/change.html',
                dependencies: [
                    'directives/app-color'
                ]
            },
            '/account/reset': {
                templateUrl: '/account/reset.html',
                dependencies: [
                    'directives/app-color'
                ]
            },
            '/account/logout': {
                templateUrl: '/account/logout.html',
                dependencies: [
                    'directives/app-color'
                ]
            },
            '/shop': {
                templateUrl: '/shop/product.html',
                dependencies: [
                    'controllers/ProductList',
                    'directives/app-color'
                ]
            }
        }
    };
});