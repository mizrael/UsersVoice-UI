(function (module) {
    var usersService = function ($http) {
	    var details = function (id) {
	        return $http({
	            method: 'GET',
	            url: '/api/users/' + id
	        });
	    };

		return {
		    details: details
		};
	};

    module.service("UsersService", ['$http', usersService]);
})(angular.module("userVoice"));