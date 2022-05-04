<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Explore Europe</title>
        <meta charset="UTF-8">
        <link rel="stylesheet" href="styles/normalize.css">
        <link rel="stylesheet" href="styles/styles.css">
        
        <?php if(isset($currentPage) && $currentPage == "map") : ?>
            <style>
                #map, .mapLayer 
                {width:<?php echo $mapWidth;?>px; 
                height:<?php echo $mapHeight;?>px;}
            </style> 
            <link rel="stylesheet" href="styles/map.css">    
        <?php endif; ?>
    </head>

    <body>
        <header>
            <img id="eu_flag" src="images/eu-flag.png" alt="European Union Flag" title="European Union Flag">
            <h1>Explore Europe</h1>
            <nav id="navigation">
                <ul>
                    <li><a href="/expeur/list.php">European Countries</a></li>
                    <li><a href="/expeur/map.php">Map of Europe</a></li>
                </ul>
            </nav>
        </header>