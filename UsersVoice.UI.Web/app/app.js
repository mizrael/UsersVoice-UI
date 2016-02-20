angular.module('userVoice', ["ngRoute"])
    .config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider.
                when('/', {
                    templateUrl: '/app/views/Main.html'
                })
                .when('/areas/:areaId/create', {
                    templateUrl: '/app/views/SubmitIdea.html',
                    controller: 'SubmitIdeaDetailController'
                })
                .when('/areas/:areaId', {
                    templateUrl: '/app/views/AreaDetail.html',
                    controller: 'AreaDetailController'
                })
                .when('/ideas/:ideaId', {
                    templateUrl: '/app/views/IdeaDetail.html',
                    controller: 'IdeaDetailController'
                })
                .when('/users/:userId', {
                    templateUrl: '/app/views/UserProfile.html',
                    controller: 'UserProfileController'
                })
                .when("/login", {
                    templateUrl: "/app/views/Login.html",
                    controller: "LoginController"
                });

        }
    ])
    .controller('AreasController', [
        '$scope', '$rootScope', '$http', 'AreasService', function($scope, $rootScope, $http, AreasService) {
            var instance = this;

            this.selectedArea = null;

            this.initialize = function() {
                AreasService.archive().then(function successCallback(response) {
                    $scope.areas = response.data.Items;
                }, function errorCallback(response) {
                    debugger;
                });
            };

            $scope.isSelected = function(area) {
                return instance.selectedArea === area;
            };

            $scope.onClickOnArea = function(area, $event) {
                instance.selectedArea = area;
                $rootScope.$broadcast('areaClicked', area);
                $event.preventDefault();
            };

            $scope.isLoading = function() {
                return null === $scope.areas || undefined === $scope.areas;
            };

            this.initialize();
        }
    ])
    .controller('IdeasController', [
        '$scope', '$http', '$location', 'LoginService', 'IdeasService', 'AreasService',
        function($scope, $http, $location, LoginService, IdeasService, AreasService) {
            var instance = this;

            instance.hasLoadedIdeas = false;

            $scope.source = null;

            this.read = function(areaId) {
                instance.hasLoadedIdeas = false;
                $scope.source = null;
                var promise;
                if (areaId) {
                    promise = AreasService.ideas(areaId);
                } else {
                    promise = IdeasService.archive({
                        sortBy: 'CreationDate',
                        sortDirection: 'DESC'
                    });
                }

                promise.then(function successCallback(response) {
                    $scope.source = response.data.Items;
                    instance.hasLoadedIdeas = ($scope.source !== null);
                });
            };

            this.initialize = function() {
                $scope.$on('areaClicked', function(event, args) {
                    $scope.selectedArea = args;
                    instance.read(args.Id, 20);
                });

                instance.read();
            };

            $scope.canAddIdea = function() {
                return (LoginService.isLogged() && true === instance.hasLoadedIdeas && $scope.selectedArea);
            };

            $scope.addNewIdeaToArea = function() {
                $location.path("/areas/" + $scope.selectedArea.Id + "/create");
            };

            $scope.isLoading = function() {
                return !instance.hasLoadedIdeas;
            };

            this.initialize();
        }
    ])
    .controller('VotingIdeaController', [
        '$scope', '$http', '$routeParams', '$rootScope', 'VotingService', 'LoginService', function($scope, $http, $routeParams, $rootScope, VotingService, LoginService) {
            var self = this;
            var user = LoginService.getUser();

            this.checkHasVoted = function() {
                VotingService.hasVoted($routeParams.ideaId).then(function(result) {
                    $scope.hasVoted = result.data;
                });
            };

            this.initialize = function() {
                self.checkHasVoted();
            };

            $scope.submitVotes = function(numberOfVotes) {
                VotingService.voteForIdea($routeParams.ideaId, numberOfVotes);
                $rootScope.$broadcast(numberOfVotes ? 'ideaVoted' : 'ideaUnvoted', numberOfVotes);
            }

            $rootScope.$on("userchanged", function(event, user) {
                self.checkHasVoted();
            });

            $scope.canVote = function() {
                return (user !== null);
            };

            this.initialize();
        }
    ])
    .controller('IdeaDetailController', [
        '$scope', '$http', '$routeParams', 'LoginService', 'UsersService', 'IdeasService',
        function($scope, $http, $routeParams, LoginService, UsersService, IdeasService) {
            var instance = this,
                isLoading = false,
                user = LoginService.getUser();

            this.readAuthorDetails = function() {
                if (!$scope.model || !$scope.model.AuthorId) {
                    return;
                }
                UsersService.details($scope.model.AuthorId)
                    .then(function(result) {
                        if (!result || !result.data) {
                            return;
                        }
                        $scope.author = result.data;
                    });
            };

            this.readModel = function() {
                isLoading = true;

                IdeasService.details($routeParams.ideaId).then(function successCallback(response) {
                    $scope.model = response.data;

                    instance.readAuthorDetails();
                }, function errorCallback(response) {
                }).then(function() {
                    isLoading = false;
                });
            };

            this.initialize = function() {
                instance.readModel();

                $scope.$on('ideaVoted', function(event, args) {
                    $scope.model.TotalPoints += args;
                });
                $scope.$on('ideaUnvoted', function(event, args) {
                    setTimeout(function() { instance.readModel(); }, 300);
                });
                $scope.$on('ideaCommented', function(event, args) {
                    $scope.model.Comments = $scope.model.Comments || [];
                    $scope.model.Comments.push(args);
                    $scope.model.TotalComments++;
                });

            };

            $scope.canViewNewCommentBox = function() {
                return (user !== null);
            };

            $scope.isLoading = function() {
                return isLoading;
            };

            this.initialize();
        }
    ])
    .controller('IdeaCommentsController', function($scope, $rootScope, $http, $routeParams, LoginService) {
        var instance = this,
            user = LoginService.getUser();

        this.initialize = function() {
            instance.resetComment();
        };

        this.resetComment = function() {
            if (!user) {
                return;
            }

            $scope.newComment = {
                Text: '',
                IdeaId: $routeParams.ideaId,
                Author: user.CompleteName,
                AuthorId: user.Id,
                CreationDate: new Date()
            };
        };

        $scope.sendComment = function() {
            if (!$scope.canSendComment()) {
                return;
            }

            $http({
                method: 'POST',
                url: "/api/ideas/" + $routeParams.ideaId + '/comment',
                data: $scope.newComment
            }).then(function successCallback(response) {
                $rootScope.$broadcast('ideaCommented', $scope.newComment);

                instance.resetComment();

            }, function errorCallback(response) {
                instance.resetComment();
            });
        };

        $scope.canSendComment = function() {
            return $scope.newComment && $scope.newComment.Text && 0 != $scope.newComment.Text.trim().length;
        };

        this.initialize();
    })
    .controller('UserProfileController', [
        '$scope', '$routeParams', 'UsersService', 'IdeasService', 'IdeaCommentsService',
        function($scope, $routeParams, UsersService, IdeasService, IdeaCommentsService) {
            var instance = this,
                isLoading = false,
                isLoadingIdeas = false,
                isLoadingComments = false;

            this.read = function() {
                if (!$routeParams.userId) {
                    return;
                }

                isLoading = true;

                UsersService.details($routeParams.userId)
                    .then(function(result) {
                        if (!result || !result.data) {
                            return;
                        }
                        $scope.model = result.data;
                        instance.readIdeas();
                        instance.readComments();
                    }).then(function() {
                        isLoading = false;
                    });
            };

            this.readIdeas = function() {
                if (!$scope.model || !$scope.model.Id) {
                    return;
                }

                isLoadingIdeas = true;

                IdeasService.archive({
                    authorId: $scope.model.Id,
                    sortBy: 'CreationDate',
                    sortDirection: 'DESC',
                    pageSize: 10
                }).then(function(result) {
                    if (!result || !result.data || !result.data.Items) {
                        return;
                    }
                    $scope.ideas = result.data.Items;
                }).then(function() {
                    isLoadingIdeas = false;
                });
            };

            this.readComments = function() {
                if (!$scope.model || !$scope.model.Id) {
                    return;
                }

                isLoadingComments = true;

                IdeaCommentsService.archive({
                    authorId: $scope.model.Id,
                    sortBy: 'CreationDate',
                    sortDirection: 'DESC',
                    pageSize: 10
                }).then(function(result) {
                    if (!result || !result.data || !result.data.Items) {
                        return;
                    }
                    $scope.comments = result.data.Items;
                }).then(function() {
                    isLoadingComments = false;
                });
            };

            this.initialize = function() {
                instance.read();

                $('#userTabs a').click(function(e) {
                    e.preventDefault();
                    $(this).tab('show');
                });
            };

            $scope.isLoading = function() {
                return isLoading;
            };
            $scope.isLoadingIdeas = function() {
                return isLoadingIdeas;
            };
            $scope.isLoadingComments = function() {
                return isLoadingComments;
            };

            this.initialize();
        }
    ])
    .controller('AreaDetailController', [
        '$scope', '$routeParams', 'AreasService', 'IdeasService',
        function ($scope, $routeParams, AreasService, IdeasService) {
            var instance = this,
                isLoading = false,
                isLoadingIdeas = false;

            this.read = function() {
                if (!$routeParams.areaId) {
                    return;
                }

                isLoading = true;

                AreasService.details($routeParams.areaId)
                    .then(function(result) {
                        if (!result || !result.data) {
                            return;
                        }
                        $scope.model = result.data;
                        instance.readIdeas();
                    }).then(function() {
                        isLoading = false;
                    });
            };

            this.readIdeas = function() {
                if (!$scope.model || !$scope.model.Id) {
                    return;
                }

                isLoadingIdeas = true;

                IdeasService.archive({
                    authorId: $scope.model.Id,
                    sortBy: 'CreationDate',
                    sortDirection: 'DESC',
                    pageSize: 10
                }).then(function(result) {
                    if (!result || !result.data || !result.data.Items) {
                        return;
                    }
                    $scope.ideas = result.data.Items;
                }).then(function() {
                    isLoadingIdeas = false;
                });
            };
    
            this.initialize = function() {
                instance.read();
            };

            $scope.isLoading = function() {
                return isLoading;
            };
            $scope.isLoadingIdeas = function() {
                return isLoadingIdeas;
            };

            this.initialize();
        }
    ])
    .controller('SubmitIdeaDetailController', function($scope, $routeParams, $http, LoginService) {
        var self = this;

        $scope.model = {
            title: '',
            description: '',
            success: false,
            validationError: false
        };

        this.validate = function() {
            return $scope.model.title && $scope.model.description;
        }

        this.cleanForm = function() {
            $scope.model.title = '';
            $scope.model.description = '';
        }

        $scope.submitIdea = function() {
            var isLogged = LoginService.isLogged();

            if (!isLogged)
                return;

            var user = LoginService.getUser();

            if (!self.validate()) {
                $scope.model.validationError = true;
                return;
            }

            $http({
                method: 'POST',
                url: '/api/ideas/',
                data: {
                    AreaId: $routeParams.areaId,
                    Title: $scope.model.title,
                    Description: $scope.model.description,
                    AuthorID: user.Id,
                    Author: user.FirstName
                }
            }).then(function successCallback(response) {
                $scope.model.validationError = false;
                $scope.model.success = true;
                self.cleanForm();
            }, function errorCallback(response) {

            });
        };
    });
