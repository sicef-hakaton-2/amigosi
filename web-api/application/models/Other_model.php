<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Other_model extends CI_Model {
	public function __construct()
    {
            parent::__construct();
    }
    
    public function getCurrentWeather()
    {
        $data = $this->locations_model->getLastLocation();
        if(!$data)          //nije ulogovan
            return false;
        $longitude = $data['longitude'];
        $latitude = $data['latitude'];
        $apikey = $this->config->item('weather_api');
        return json_decode(file_get_contents("http://api.openweathermap.org/data/2.5/weather?lat={$latitude}&lon={$longitude}&appid={$apikey}"));
    }    
    public function getCurrencyList()
    {
      /*  $data = $this->locations_model->getLastLocation();
        if(!$data)          //nije ulogovan
            return false;
        $longitude = $data['longitude'];
        $latitude = $data['latitude'];*/

        $apikey = $this->config->item('currency_api');
        //echo $apikey;
        $data = json_decode(file_get_contents("http://api.kursna-lista.info/{$apikey}/kursna_lista/json"));
        // print_r($data);
        return array("eur" => $data->result->eur->sre, "usd" => $data->result->usd->sre, "chf" => $data->result->chf->sre, "gbp" => $data->result->gbp->sre);
    }    

    public function getQRCode()
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;
        return "http://api.qrserver.com/v1/create-qr-code/?size=150x150&data=$gid";
        //return file_get_contents("http://api.qrserver.com/v1/create-qr-code/?size=150x150&data=$gid");
    }
}