<?php

/**
 * Class RouterController
 */
class RouterController extends Controller {
    protected $controller;

    /**
     * @param mixed $parameters
     * 
     * @return void
     */
    public function run($parameters) {

        $parser = new Parser();

        $parsedUrl = $parser->parse($parameters[0]);

        if (empty($parsedUrl[0]))		
			$this->route('homepage');	

        $controllerName = $parser->camelCase(array_shift($parsedUrl)) . 'Controller';

        if ( file_exists('controllers/' . $controllerName . '.php'))
			$this->controller = new $controllerName;
		else
			$this->route('error');
		
        $this->controller->run($parsedUrl);
		
		$this->data['title'] = $this->controller->header['title'];
		$this->data['description'] = $this->controller->header['description'];
		$this->data['keywords'] = $this->controller->header['keywords'];
		
		$this->view = 'template';


    }


}