angular.module('beamng.apps')
.directive('RandomSpawn', [function (){ 
	return {
		//Define the UI layout. TODO: See if the game still accepts HTML imported from a file
		template: '<div id="randApp" style="max-height:100%; width:100%; margin:15px; background:transparent;" layout="row" layout-align="center left" layout-wrap>' +
			
			'<div id="randApp" style="">' +
		
				'<md-button id="camsoToggle" value="true" style="width: 50px; height: 50px; background: #90FF88BB;" md-no-ink class="md-fab" ng-click="toggleButtonClick($event)"> <md-tooltip id="camsoTip">Toggle Automation Vehicles</md-tooltip> <md-icon style="position:relative; left: 0px; bottom: 4px; width: 85%; height: 85%; fill: #EEEEEE;" md-svg-src="/ui/modules/apps/RandomVehConfig/camso.svg"></md-icon></md-button>' +
			
				'<md-button flex style="margin: 2px; min-width: 18ch; background: #AAAABBBB;" md-no-ink class="md-raised" ng-click="spawnRandVeh()" >Random vehicle</md-button>' +
				
			'</div>' +
			'<div id="randApp" style="">' +
			
				'<md-button id="weighToggle" value="true" style="width: 50px; height: 50px; background:#90FF88BB;" md-no-ink class="md-fab" ng-click="toggleButtonClick($event)"><md-tooltip>Toggle Fair Config Rolls</md-tooltip><md-icon style="position:relative; left: 0px; bottom: 4px; width:85%; height:85%; fill: #EEEEEE;" md-svg-src="/ui/modules/apps/RandomVehConfig/equal.svg"></md-icon></md-button>' +
			
				'<md-button flex style="margin: 2px; min-width: 18ch; background: #AAAABBBB;" md-no-ink class="md-raised" ng-click="spawnRandConfig()">Random config</md-button>' +			
			'</div>' +
			
		'</div>',
		
		replace: true,
		restrict: 'EA',
		link: function (scope, element, attrs) 
		{
			element.ready(function () {
				//Todo: Find a way to store toggle states
				//ctrl.getSettings is deprecated 
			});
			
			//LUA engine call for the random vehicle button
			scope.spawnRandVeh = function() {
				//Check spawn options
				automationToggle = document.getElementById("camsoToggle");
				automationBool = automationToggle.value;
				
				bngApi.engineLua('extensions.core_BRVCspawn.randVehicle('+automationBool+')');
			}
			
			//LUA engine call for the random vehicle-config button
			scope.spawnRandConfig = function() {
				//Check spawn options
				automationToggle = document.getElementById("camsoToggle");
				fairSpawnsToggle = document.getElementById("weighToggle");
				automationBool = automationToggle.value;
				fairSpawnsBool = fairSpawnsToggle.value;
				
				bngApi.engineLua('extensions.core_BRVCspawn.randConfig('+automationBool+','+fairSpawnsBool+')');
			}
			
			//UI art update for clicking red/green toggle buttons
			scope.toggleButtonClick = function(button) {
				clickedButton = document.getElementById(button.currentTarget.id);
								
				if (clickedButton.value == "true"){
					clickedButton.setAttribute("value", "false");
					clickedButton.style.background = "#FF9088BB";
				}
				else{
					clickedButton.setAttribute("value", "true");
					clickedButton.style.background = "#90FF88BB";
				}
			}
			
			scope.$on('$destroy', function () {
				
			});
		}
	};
   }
  ]
 );