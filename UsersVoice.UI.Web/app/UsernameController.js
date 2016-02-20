(function (module) {

	var usernameController = function($rootScope,$scope, LoginService) {
		$scope.username = "";
		$scope.points = 0;
		
		$rootScope.$on("userchanged", function (event, user) {
		    console.log("on user changed", user);
		    if (user) {
		    	$scope.username = user.FirstName;
			    $scope.points = user.AvailablePoints;
		    }
		});

	    $scope.isLogged = function() {
	        return LoginService.isLogged();
	    };
	};

	module.controller("UsernameController", ["$rootScope","$scope","LoginService", usernameController]);

})(angular.module("userVoice"));