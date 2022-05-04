<?php $mapWidth = 740;
      $mapHeight = 740;
      $currentPage = basename(__FILE__, ".php");

      include('includes/header.php'); 
?>

    <script type="text/javascript">
        const mapWidth = <?php echo $mapWidth;?>;
        const mapHeight = <?php echo $mapHeight;?>;
    </script>

    <script src="scripts/mapHandler.js"></script>
    <main>
        <div id="map" onclick="mapClick(event)" onmousemove="mapHover(event)"
                        onmouseover="mapEnter()" onmouseout="mapExit()">
            
            <?php
                //Create individual canvas objects for each country that we will be supporting
                //This is used to create a clickable map
                $countryCodes = file_get_contents('data/country-codes.json');
                $countryCodes = json_decode($countryCodes, true);
                foreach ($countryCodes as $country)
                {
                    echo "\n" . '<canvas name="' . $country['name'] . 
                                        '" id="layer_' .$country['alpha3Code'] . 
                                        '" width="' . $mapWidth .
                                        '" height="' . $mapHeight .
                                        '" class="mapLayer">';

                    echo "\n     " . '<img id="img_' . $country['alpha3Code'] .
                                        '" src="images/map/' .$country['alpha3Code'] . '.png' .
                                        '" width="'  . $mapWidth .
                                        '" height="' . $mapHeight .'">';

                    echo "\n" . '</canvas>' . "\n";
                }
            ?> 
        </div>

        <div id="hover_container">
            <div id="hover_indicator" hidden></div>
        </div>
    </main>
<?php include('includes/footer.php'); ?>