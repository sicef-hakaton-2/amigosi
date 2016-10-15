<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Camps_model extends CI_Model {
	public function __construct()
    {
            parent::__construct();
    }
   /* public function setCamp()
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
    }*/

   public function getAllCamps()	
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;

        $q = $this->db->query("SELECT * FROM camps ORDER BY id");
        
        if(!$q->num_rows())
            return false;

        return $q->result_array();
    }
    public function getNearbyCamps()
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;

        $location = $this->locations_model->getLastLocation();
        if(!$location)
            return false;

        $allcamps = $this->getAllCamps();
        //print_r($allcamps);
        if(!$allcamps)
            return 404;

        //print_r($allcamps);
       
        for($i = 0; $i < count($allcamps); $i++)
        {
            //print_r($allcamps[$i]);
            $allcamps[$i]['distance'] = getDistanceBetweenPointsNew($location['latitude'],$location['longitude'],$allcamps[$i]['latitude'],$allcamps[$i]['longitude']);
            $allcamps[$i]['units']  = "Km";
            $allcamps[$i]['numbergoingto']  = $this->getNumberOfPeopleToCamp($allcamps[$i]['id']);
            $allcamps[$i]['numberthere']    = $this->getNumberOfPeopleAtCamp($allcamps[$i]['id']);
        }

        //sort
        for($i = 0; $i < count($allcamps); $i++)
        {
            for($j = 0; $j < count($allcamps) - 1; $j++)
            {
                if($allcamps[$j]['distance'] > $allcamps[$j+1]['distance'])
                {
                    $tmp = $allcamps[$j+1];
                    $allcamps[$j+1] = $allcamps[$j];
                    $allcamps[$j] = $tmp;
                }   
            }
        }
       // array_sort($allcamps,'distance',SORT_ASC);
        return $allcamps;
    }
    //---------------------------------------------------
    public function getNumberOfPeopleToCamp($c='')
    {

        if(!isset($_POST['campid']) && empty($_POST['campid']))
            $campid = $c;
        else
            $c = $_POST['campid'];

        $campid = $this->db->escape($c);
        $num = 0;
        $q = $this->db->query("SELECT SUM(users.numchildren) FROM users,destinationcamp WHERE users.groupid = destinationcamp.groupid && campid = {$campid} && incamp = '0' && leftcamp = '0';");
        $row = $q->row_array();
        $num += (int)$row['SUM(users.numchildren)'];
        $q = $this->db->query("SELECT SUM(users.numadults) FROM users,destinationcamp WHERE users.groupid = destinationcamp.groupid && campid = {$campid} && incamp = '0' && leftcamp = '0';");
        $row = $q->row_array();
        $num += (int)$row['SUM(users.numadults)'];

        return $num;
    }
    public function getNumberOfPeopleAtCamp($c='')
    {

        if(!isset($_POST['campid']) && empty($_POST['campid']))
            $campid = $c;
        else
            $c = $_POST['campid'];

        $campid = $this->db->escape($c);
        $num = 0;
        $q = $this->db->query("SELECT SUM(users.numchildren) FROM users,destinationcamp WHERE users.groupid = destinationcamp.groupid && campid = {$campid} && incamp = '1' && leftcamp = '0';");
        $row = $q->row_array();
        $num += (int)$row['SUM(users.numchildren)'];
        $q = $this->db->query("SELECT SUM(users.numadults) FROM users,destinationcamp WHERE users.groupid = destinationcamp.groupid && campid = {$campid} && incamp = '1' && leftcamp = '0';");
        $row = $q->row_array();
        $num += (int)$row['SUM(users.numadults)'];

        return $num;
    }
    public function setCampDestination()    //IMEI, IDkampa
    {
        $gid = $this->user_model->getUserData()['groupid'];
        if(!$gid) //ako nije ulogovan
            return false;
        //    echo "1";
        if(!isset($_POST['campid']) && empty($_POST['campid']))
            return false;
        //echo "2";
        $campid = $this->db->escape($_POST['campid']);

        //da li vec ima jednu destinaciju
        $q = $this->db->query("SELECT id FROM destinationcamp WHERE groupid = '{$gid}' && leftcamp = '0' LIMIT 1;");
        
        if($q->num_rows())  //vec je u  kretanju kampu brisi ga
        {
            $row = $q->row_array();
            $this->db->query("DELETE FROM destinationcamp WHERE id = '{$row['id']}'"); //brisemo iz baze i onda dodajemo novu destinaciju kamp
        }
        //echo "3";
        $this->db->query("INSERT INTO destinationcamp (id,campid,groupid) VALUES(NULL,{$campid},'{$gid}')");
        return true;
    }
    //-----------------------!SALTER!-------------------------
    public function existsShelter($name,$location,$latitude,$longitude) //ne
    {
        //pronadji da li ima neki vec takav identican salter
        $name = $this->db->escape($name);
        $location = $this->db->escape($location);
        $latitude = $this->db->escape($latitude);
        $longitude = $this->db->escape($longitude);
        $q = $this->db->query("SELECT 1 FROM camps WHERE name = {$name} && location = {$location} && latitude = {$latitude} && longitude = {$longitude};");
        if(!$q->num_rows())
            return false;
        return true;
    }
    public function setInShelter() //radi salter TOKEN
    {
        if(!$this->tokenauth_model->isAuthorised())
            return false;
        if(!isset($_POST['campid']) && empty($_POST['campid']))
            return false;
        if(!isset($_POST['groupid']) && empty($_POST['groupid']))
            return false;
        $campid = $this->db->escape($_POST['campid']);
        $groupid = $this->db->escape($_POST['groupid']);
        $this->db->query("UPDATE destinationcamp SET incamp = 1, leftcamp = 0 WHERE campid = {$campid} && groupid = {$groupid};");
        return true;
    }
    public function setOutShelter() //radi salter TOKEN 
    {
        if(!$this->tokenauth_model->isAuthorised())
            return false;
        if(!isset($_POST['campid']) && empty($_POST['campid']))
            return false;
        if(!isset($_POST['groupid']) && empty($_POST['groupid']))
            return false;
        $campid = $this->db->escape($_POST['campid']);
        $groupid = $this->db->escape($_POST['groupid']);
        $this->db->query("UPDATE destinationcamp SET leftcamp = 1, incamp = 0 WHERE campid = {$campid} && groupid = {$groupid};");
        return true;
    }
    public function addCamp() //radi salter TOKEN
    {

        if(!$this->tokenauth_model->isAuthorised())
            return false;
        if(!isset($_POST['name']) && empty($_POST['name']))
            return false;
        if(!isset($_POST['location']) && empty($_POST['location']))
            return false;
        if(!isset($_POST['longitude']) && empty($_POST['longitude']))
            return false;
        if(!isset($_POST['latitude']) && empty($_POST['latitude']))
            return false;
        $name = $this->db->escape_str($_POST['name']);
        $location = $this->db->escape_str($_POST['location']);
        $longitude = $this->db->escape_str($_POST['longitude']);
        $latitude = $this->db->escape_str($_POST['latitude']);

        if($this->existsShelter($name,$location,$latitude,$longitude))
            return false;

        $this->db->query("INSERT INTO camps (id,name,location,longitude,latitude,capacity) VALUES(NULL,'$name','$location','$longitude','$latitude',0)");
        return true;
    }

        
}