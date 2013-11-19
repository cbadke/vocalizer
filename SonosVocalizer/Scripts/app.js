"use strict"
var app = angular.module('vocalize', []);

app.controller('home', function ($scope, $http, $window) {
    $scope.phrase = "";
    $scope.execute = function () {
        $http.post('/api/testvocalize', { phrase : $scope.phrase, voice : $scope.voice })
        .then(function (resp) {
            $scope.audioStream = '/api/testvocalize/' + resp.data.id;
        });
    };
    $scope.audioStream = null;

    $scope.voices = [];
    $scope.voice = "";
    $http.get('/api/voices')
         .then(function (resp) {
             if (resp.data && resp.data.length) {
                 $scope.voices = resp.data.map(function (v) { return v.name;});
                 $scope.voice = $scope.voices[0];
             }
         });

});