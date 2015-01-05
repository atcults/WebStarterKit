define(['app'], function(app)
{
    app.controller('HomeAboutCtrl',
    [
        '$scope',

        function($scope)
        {
            $scope.page =
            {
                heading: 'About Us'
            };
        }
    ]);
});