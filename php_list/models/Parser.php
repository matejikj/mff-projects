<?php

class Parser {

    function __construct() {
    }

    public function camelCase($url) {
        $path = str_replace('-', ' ', $url);
        $path = ucwords($url);
        $path = str_replace(' ', '', $path);
        return $path;
    }

    public function parse($url) {
        $parsedUrl = parse_url($url);

        $parsedUrl["path"] = ltrim($parsedUrl["path"], "/");

        $parsedUrl["path"] = trim($parsedUrl["path"]);

        $path = explode("/", $parsedUrl["path"]);
        return $path;
    }
}