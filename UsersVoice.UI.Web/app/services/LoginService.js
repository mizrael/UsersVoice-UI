(function (module) {
	var loginService = function() {
	    var currentUser = null;

		var setUser = function(user) {
			currentUser = user;
		};

		var getUser = function() {
			return currentUser;
		};

		var isLogged = function () {
		    return currentUser !== null;
		};

		return {
		    isLogged: isLogged,
			getUser: getUser,
			setUser: setUser
		};
	};

	module.service("LoginService", loginService);
})(angular.module("userVoice"));