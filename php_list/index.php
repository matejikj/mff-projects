<?php

mb_internal_encoding("UTF-8");

/**
 * @param $name
 */
function classesAutoload($name) {
   if (preg_match('/Controller$/', $name)) {
      require("controllers/" . $name . ".php");
   } else {
      require('models/' . $name . ".php");
   }
}

spl_autoload_register("classesAutoload");

require_once 'db_config.php';

DB::connectDB($db_config);

if( !empty($_SERVER['HTTP_X_REQUESTED_WITH']) && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest') {
   $ajax = new AJAXController();
   $ajax->run(array($_SERVER['REQUEST_URI']));

   $ajax->showView();
} else {
   $router = new RouterController();
   $router->run(array($_SERVER['REQUEST_URI']));

   $router->showView();
}