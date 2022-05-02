local M  = {}

-- Spawns a random vehicle in default configuration
-- Automation vehicles can be removed from the spawn pool
local function randVehicle(boolCamso)
	local numVehicles = #core_vehicles.getModelList(true).models
	local chosenVehicle = core_vehicles.getModelList(true).models[math.random(1,numVehicles)]
	
	-- Reroll away undesired results
	while (chosenVehicle.Type == 'Prop' or 
		    chosenVehicle.Type == 'Trailer' or
		    boolCamso == false and 
		    chosenVehicle.Type == 'Automation') do
		--print('Rerolled ',chosenVehicle.Name)
		
		chosenVehicle = core_vehicles.getModelList(true).models[math.random(1,numVehicles)]
	end
	
	core_vehicles.replaceVehicle(chosenVehicle.key,{})
	ui_message('Spawned: '..chosenVehicle.Name)
end

-- Spawn a random vehicle in random config
-- In fair mode, chooses a random vehicle, then one of its configs randomly
-- In unfair mode, chooses a random config from the global set
--- cars with many configs will spawn more frequently in unfair mode
-- Automation vehicles can be removed from the spawn pool
local function randConfig(boolCamso, boolFair)

	local chosenConfig = ''
	
	-- Fair mode
	if (boolFair == true) then
		
		local numVehicles = #core_vehicles.getModelList(true).models
		local chosenModel = core_vehicles.getModelList(true).models[math.random(1,numVehicles)]
		
		-- Choose a single vehicle model
		-- Reroll away undesired results
		while (chosenModel.Type == 'Prop' or 
				chosenModel.Type == 'Trailer' or
				boolCamso == false and 
				chosenModel.Type == 'Automation') do
			
			chosenModel = core_vehicles.getModelList(true).models[math.random(1,numVehicles)]
		end 
		
		local allConfigs = core_vehicles.getConfigList(true)
		local modelConfigs = {}
		
		-- Build a list of configs for the chosen model
		for i,v in pairs(allConfigs.configs) do
			if (v.model_key == chosenModel.key) then
				table.insert(modelConfigs, {key = v.key, name = v.Name})
			end
		end

		-- Randomly choose a config
		if (#modelConfigs == 0) then ui_message("No configs found for "..chosenModel.Name)
		else chosenConfig = modelConfigs[math.random(1,#modelConfigs)]
		end
	
		-- Spawn the vehicle
		core_vehicles.replaceVehicle(chosenModel.key, {config = chosenConfig.key})
		ui_message('Spawned: '..chosenConfig.name)
	
		-- Create a log for examining spawn frequencies
		--print("Fairmode: "..chosenModel.key)

	-- Unfair mode
	else
		local allConfigs = core_vehicles.getConfigList(true)
		local numConfigs = #allConfigs.configs
		chosenConfig = allConfigs.configs[math.random(1,numConfigs)]
		--Roll away undesired results
		while (chosenConfig.aggregates.Type.Prop == true or
				chosenConfig.aggregates.Type.Trailer == true or
				boolCamso == false and 
				chosenConfig.aggregates.Type.Automation == true) do
			--print('Rerolled ',chosenConfig.Name)
		
			chosenConfig = allConfigs.configs[math.random(1,numConfigs)]
		end
		
		-- Spawn the vehicle
		core_vehicles.replaceVehicle(chosenConfig.model_key, {config = chosenConfig.key})
		ui_message('Spawned: '..chosenConfig.Name)
		
		-- Create a log for examining spawn frequencies
		--print("Rawmode: "..chosenConfig.model_key)
	end
end

M.randVehicle = randVehicle
M.randConfig = randConfig
return M