require.config({
    baseUrl: '/scripts',
    paths: {
		'angular': '/bower_components/angular/angular',
		'angular-route': '/bower_components/angular-route/angular-route',
		'angular-local-storage': '/bower_components/angular-local-storage/dist/angular-local-storage',
		'angular-loading-bar': '/bower_components/angular-loading-bar/build/loading-bar',
		'bootstrap-autohidingnavbar': '/bower_components/bootstrap-autohidingnavbar/src/jquery.bootstrap-autohidingnavbar',
		'jquery': '/bower_components/jquery/dist/jquery',
		'twitter-bootstrap': '/bower_components/bootstrap/dist/js/bootstrap',
		'angular-sanitize': '/bower_components/angular-sanitize/angular-sanitize.min'
    },
	shim: {
		'app': {
			deps: ['angular', 'angular-route','angular-sanitize', 'angular-local-storage', 'angular-loading-bar', 'twitter-bootstrap','bootstrap-autohidingnavbar']
		},
		'angular-route': {
			deps: ['angular']
		},
		'angular-local-storage': {
			deps: ['angular']
		},
		'angular-loading-bar': {
			deps: ['angular']
		},
		'twitter-bootstrap': {
			deps: ['jquery']
		},
		'bootstrap-autohidingnavbar': {
			deps: ['jquery']
		},
		'angular-sanitize': {
			deps:['angular']
		}
	}
});

require
(
    [
        'app', 'routes','services/dependencyResolverFor', 'services/authService', 'extensions/stringExt'
    ],
    function(app, config, dependencyResolverFor)
    {
		app.config(
			[
				'$routeProvider',
				'$locationProvider',
				'$controllerProvider',
				'$compileProvider',
				'$filterProvider',
				'$provide',
				'$httpProvider',

				function($routeProvider, $locationProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider)
				{
					app.controller = $controllerProvider.register;
					app.directive  = $compileProvider.directive;
					app.filter     = $filterProvider.register;
					app.factory    = $provide.factory;
					app.service    = $provide.service;

					$locationProvider.html5Mode({
						enabled: true,
						requireBase: false
					});

					if(config.routes !== undefined)
					{
						angular.forEach(config.routes, function(route, path)
						{
							$routeProvider.when(path, {templateUrl:route.templateUrl, resolve:dependencyResolverFor(route.dependencies)});
						});
					}

					if(config.defaultRoutePaths !== undefined)
					{
						$routeProvider.otherwise({redirectTo:config.defaultRoutePaths});
					}

					$httpProvider.interceptors.push('httpRequestInterceptor');

				}
			]);

		app.run(['authService', function (authService) {
			console.log("app run auth");
			authService.fillAuthData();
		}]);

		app.run(function ($rootScope, $location) {
			$rootScope.$on("$routeChangeSuccess", function (currentRoute, previousRoute) {
				$rootScope.title = "Title";// currentRoute.title;
			});
		});

		app.controller('AppMainCtrl',
			[
				'$scope', '$route', '$routeParams', '$location', 'authService',

				function($scope, $route, $routeParams, $location, authService)
				{
					$scope.authentication = authService.authentication;

					$scope.showHeaderFooter = true;
					$scope.addCss = false;

					$scope.$route = $route;
					$scope.$location = $location;
					$scope.$routeParams = $routeParams;

					$scope.page =
					{
						heading: "",
						message : undefined
					};

					$scope.$watch('$location.path()', function(path) {
						console.log(path);
						$scope.message = undefined;

						if(path.startsWith('/account')){
							$scope.showHeaderFooter = false;
							$scope.addCss = true;
						} else{
							$scope.addCss = false;
							$scope.showHeaderFooter = true;
						}

						if($location.path() == '/account/login'){
							$scope.page.heading = 'Login';
						}else if($location.path() == '/account/signup'){
							$scope.page.heading = 'Sign Up';
						}else if($location.path() == '/account/change'){
							$scope.page.heading = 'Change Password';
						}else if($location.path() == '/account/reset'){
							$scope.page.heading = 'Reset Password';
						}else if($location.path() == '/account/logout'){
							$scope.page.heading = 'Logout';
						}
					});

					//Login Page

					$scope.loginData =
					{
						userName: "admin",
						password: "admin",
						useRefreshTokens: true
					};

					$scope.login = function () {
						authService.login($scope.loginData).then(function (response) {
								$location.path('/');
							},
							function (err) {
								$scope.message = err.error_description;
							});
					};

					// Logout Page
					$scope.logOut = function () {
						authService.logOut();
						$location.path('/account/logout');
					};

					$scope.menu = {"menuItems" : [
						{"name" : "Mobile", "prURL" : "MobileLink"},
						{"name" : "Tablets", "prURL" : "TabletLink"},
						{"name" : "Mobile Accessories", "prURL" : "MAccessoriesLink"},
						{"name" : "Laptop", "prURL" : "LaptopLink"},
						{"name" : "Laptop Accessories", "prURL" : "LAccessoriesLink"}
					]};

					// New Menu Code Added Start
					$scope.affixed = 'top';
					$scope.search = {
						show : true,
						terms : ''
					};
					$scope.brand = "<span class='glyphicon glyphicon-user'></span> Brand";
					$scope.inverse = true;
					$scope.menus = [

						{
							title : "About Us",
							url : "/about"


						},
						{
							title : "Contact Us",
							url : "/contact"
						},
						{
							title : "Start Shopping",
							url : "/shop"
						},
						{	title : " Staff Station",
							menu : [
								{
									title : "Checkout",
									action : "",
									url: "/"

								},
								{
									title : "CheckIn",
									action : "",
									url: "/"
								},

								{
									title : "Renew",
									action : "",
									url: "/"
								},
								{
									title : "Loss of Item",
									action : "",
									url: "/"
								},

								{
									title: "another test",
									menu:[
										{
											title : "Menu Item Inner",
											action : "item.in"

										},
										{
											divider: true
										}
									]
								}
							]
						}
					]; // end menus

					$scope.item = '';
					$scope.styling = 'Inverse';
					$scope.searchDisplay = 'Visible';

					$scope.searchfn = function(){
						alert('Attempting search on: "' + $scope.search.terms + '"');
					}; // searchfn

					$scope.navfn = function(action){
						switch(action){
							case 'item.one':
								$scope.item = 'Item one selected.';
								break;
							case 'item.two':
								$scope.item = 'Item two selected.';
								break;
							case 'item.three':
								$scope.item = 'Item three selected.';
								break;
							case 'singular':
								$scope.item = 'Singular link item selected.';
								break;
							default:
								$scope.item = 'Default selection.';
								break;
						};

						// $scope.item = action;

						// end switch
					}; // end navfn

					$scope.toggleStyling = function(){
						$scope.inverse = !$scope.inverse;
						if(angular.equals($scope.inverse,true))
							$scope.styling = 'Inverse';
						else
							$scope.styling = 'Default';
					}; // end toggleStyling

					$scope.toggleSearchForm = function(){
						$scope.search.show = !$scope.search.show;
						if(angular.equals($scope.search.show,true))
							$scope.searchDisplay = 'Visible';
						else
							$scope.searchDisplay = 'Hidden';
					}; // end toggleSearchForm

					$scope.addMenu = function(){
						$scope.menus.push({
							title : "Added On The Fly!",
							action : "default"
						});
					}; // end test

					$scope.toggleAffixed = function(){
						switch($scope.affixed){
							case 'top':
								$scope.affixed = 'bottom';
								break;
							case 'bottom':
								$scope.affixed = 'none';
								break;
							case 'none':
								$scope.affixed = 'top';
								break;
						};
					};

					// New Menu Code Added End
					$("div.navbar-fixed-top").autoHidingNavbar();

				}
			]);

		app.directive('angledNavbar',function(){
			return {
				restrict : 'AE',
				scope : {
					brand : '=',
					menus : '=',
					affixed : '=',
					search : '=',
					searchfn : '&',
					navfn : '&',
					inverse : '='
				},
				//templateUrl : 'tmpls/nav/navbar.html',
				templateUrl : '../navbar.html',
				controller : function($scope,$element,$attrs){
					//=== Scope/Attributes Defaults ===//

					$scope.defaults = {
						brand : '<span class="glyphicon glyphicon-certificate"></span>',
						menus : [],
						search : {
							show : false
						}
					}; // end defaults

					// if no parent function was passed to directive for navfn, then create one to emit an event
					if(angular.isUndefined($attrs.navfn)){
						$scope.navfn = function(action){
							if(angular.isObject(action))
								$scope.$emit('nav.menu',action);
							else
								$scope.$emit('nav.menu',{'action' : action});
						}; // end navfn
					}

					// if no parent function was passed to directive for searchfn, then create one to emit a search event
					if(angular.isUndefined($attrs.searchfn)){
						$scope.searchfn = function(){
							$scope.$emit('nav.search.execute');
						}; // end searchfn
					}

					//=== Observers & Listeners ===//

					$scope.$watch('affixed',function(val,old){
						var b = angular.element(document).find('body');
						// affixed top
						if(angular.equals(val,'top') && !b.hasClass('navbar-affixed-top')){
							if(b.hasClass('navbar-affixed-bottom'))
								b.removeClass('navbar-affixed-bottom');
							b.addClass('navbar-affixed-top');
							//affixed bottom
						}else if(angular.equals(val,'bottom') && !b.hasClass('navbar-affixed-bottom')){
							if(b.hasClass('navbar-affixed-top'))
								b.removeClass('navbar-affixed-top');
							b.addClass('navbar-affixed-bottom');
							// not affixed
						}else{
							if(b.hasClass('navbar-affixed-top'))
								b.removeClass('navbar-affixed-top');
							if(b.hasClass('navbar-affixed-bottom'))
								b.removeClass('navbar-affixed-bottom');
						}
					}); // end watch(affixed)

					//=== Methods ===//

					$scope.noop = function(){
						angular.noop();
					}; // end noop

					$scope.navAction = function(action){
						$scope.navfn({'action' : action});
					}; // end navAction

					/**
					 * Have Branding
					 * Checks to see if the "brand" attribute was passed, if not use the default
					 * @result  string
					 */
					$scope.haveBranding = function(){
						return (angular.isDefined($attrs.brand)) ? $scope.brand : $scope.defaults.brand;
					};

					/**
					 * Has Menus
					 * Checks to see if there were menus passed in for the navbar.
					 * @result  boolean
					 */
					$scope.hasMenus = function(){
						return (angular.isDefined($attrs.menus));
					};

					/**
					 * Has Dropdown Menu
					 * Check to see if navbar item should have a dropdown menu
					 * @param  object  menu
					 * @result  boolean
					 */
					$scope.hasDropdownMenu = function(menu){
						return (angular.isDefined(menu.menu) && angular.isArray(menu.menu));
					}; // end hasDropdownMenu

					/**
					 * Is Divider
					 * Check to see if dropdown menu item is to be a menu divider.
					 * @param  object  item
					 * @result  boolean
					 */
					$scope.isDivider = function(item){
						return (angular.isDefined(item.divider) && angular.equals(item.divider,true));
					}; // end isDivider
				}
			};
		})

		// End of Directive

        angular.bootstrap(document, ['app']);

		$("div.navbar-fixed-top").autoHidingNavbar({
		});

		//Set timeout will set to 0 in production use
		setTimeout(function(){
			$("#splash").fadeOut();
			$('link[rel=stylesheet][href~="styles/splash.css"]').remove();
		//	$("#splashCss").attr("disabled", "disabled");
			$("#page").fadeIn();
		}, 3000);
	}
);