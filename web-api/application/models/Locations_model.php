<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Locations_model extends CI_Model {
	public function __construct()
    {
            parent::__construct();
    }
    public function setLastLocation()
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;

        if(isset($_POST['longitude']) && !empty($_POST['longitude']) &&
            isset($_POST['latitude']) && !empty($_POST['latitude']) &&
            isset($_POST['city']) &&
             isset($_POST['gps'])
            )
        {
            $longitude  = $this->db->escape($_POST['longitude']);
            $latitude   = $this->db->escape($_POST['latitude']);
            $city       = $this->db->escape($_POST['city']);
            $gps        = $this->db->escape($_POST['gps']);
            //sve ok
            $this->db->query("INSERT INTO lastlocation (id,groupid,longitude,latitude,city,gps) VALUES (NULL,'{$gid}',$longitude,$latitude,$city,$gps)");
            return true;
        }
        return false;
    }
    public function getLastLocation()	
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;
        $q = $this->db->query("SELECT * FROM lastlocation WHERE groupid = '{$gid}' ORDER BY id DESC LIMIT 1;");
        
        if(!$q->num_rows())
            return false;

        return $q->row_array();
    }
    public function getAllLocations()
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;

        $q = $this->db->query("SELECT * FROM lastlocation WHERE groupid = '{$gid}' ORDER BY id DESC;");
        
        if(!$q->num_rows())
            return false;
        
        return $q->result_array();
    }
    
        
}