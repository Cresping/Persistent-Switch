using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Persistence
{
    /// <summary>
    /// Clase encarga de guardar y cargar la información del juego, además de realizar comprobaciones sobre los ficheros
    /// </summary>
    [Serializable]
    public class FileManager
    {
        //Este es el archivo real donde se almacenará la información
        [SerializeField] private string fileNameSaveData = "PersistentSwitch.RIP";
        //Este archivo es usado como desencadenante del Scare en el menú, para más información ver la clase MenuController
        [SerializeField] private string fileNameScare = "Graves.RIP";

        /// <summary>
        /// Devuelve si existen archivos de guardado
        /// </summary>
        /// <returns>True si existe, false si no</returns>
        public bool CheckGameDataExists()
        {
            return File.Exists(Path.Combine(Application.persistentDataPath, fileNameSaveData));

        }
        /// <summary>
        /// Devuelve si existe el archivo desencadenante del Scare
        /// </summary>
        /// <returns>True si existe, false si no </returns>
        public bool CheckScareExists()
        {
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileNameScare));
        }
        /// <summary>
        /// Serializa los datos del juego
        /// </summary>
        /// <param name="gameData">Datos del juego</param>
        public void SerializeGameData(GameData gameData)
        {
            //El archivo desencadenante del Scare contiene los datos del juego y es almacenado en un sitio donde la mayoría de usuarios saben acceder
            FileStream fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileNameScare), FileMode.Create);
            BinaryFormatter formater = new BinaryFormatter();
            formater.Serialize(fs, gameData);
            fs.Close();
            //Por otro lado, el verdadero archivo de guardado se almacena en un sitio donde la mayoría de usuarios no saben acceder
            fs = new FileStream(Path.Combine(Application.persistentDataPath, fileNameSaveData), FileMode.Create);
            formater.Serialize(fs, gameData);
            fs.Close();
        }
        /// <summary>
        /// Deserializa los datos del juego y los devuelve
        /// </summary>
        /// <returns>Datos del juego</returns>
        public GameData DeSereializeGameData()
        {
            FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, fileNameSaveData), FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            GameData gameData = (GameData)formatter.Deserialize(fs);
            fs.Close();
            return gameData;
        }
        public String FileNameScare
        {
            get
            {
                return fileNameScare;
            }
        }
    }
}
   
