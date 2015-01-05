'use strict';

module('app', function(app)
{
	console.log(app)
});

define(['app'], function(app)
{
	describe("The 'HomeViewController'", function()
	{
		var $rootScope;
		var $controller;
		var $scope;

		beforeEach(function()
		{
			module('app');

			inject
			([
				'$injector',
				'$rootScope',
				'$controller',

				function($injector, _$rootScope, _$controller)
				{
					$rootScope = _$rootScope;
					$scope = $rootScope.$new();
					$controller = _$controller;
				}
			]);

			$controller('HomeIndexCtrl', {$scope: $scope});
		});

		it("should set the page heading to 'Welcome'", function()
		{
			expect($scope.page.heading).toBe('Welcome');
		});
	});
});