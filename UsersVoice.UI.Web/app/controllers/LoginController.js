(function (module) {

	var loginController = function($http, $rootScope,$scope,$location,$loginService) {
	    $scope.username = "";
	    $scope.isLoading = false;
	    $scope.error = false;

		$scope.login = function () {
		    $scope.isLoading = true;
		    $scope.error = false;
		    console.log($scope.username);

		    $http({
		        method: "POST",
		        url: "api/Login",
		        data: { userName: $scope.username }
		    }).then(function success(response) {
		        $scope.isLoading = false;
		        $scope.error = (!response || 200 !== response.statusCode);

		        $loginService.setUser(response.data);
		        $rootScope.$broadcast("userchanged", response.data);
		        $location.path("/");
		    }).catch(function() {
		        $scope.isLoading = false;
		        $scope.error = true;
		    });


		    //$loginService.setUser($scope.username);
		    //$rootScope.$broadcast("userchanged", $scope.username);
		    //$location.path("/");
		};
	};

	module.controller("LoginController",["$http","$rootScope","$scope","$location","LoginService",loginController]);

})(angular.module("userVoice"));