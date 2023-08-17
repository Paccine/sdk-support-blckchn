using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Atavism
{
    /// <summary>
    /// Este módulo contém os diversos métodos para lidar com o alvo
    /// </summary>
    public class AtavismTarget
    {
        #region API de Alvo

        /// <summary>
        /// Método para atacar o alvo pelo ID do objeto.
        /// </summary>
        /// <param name="objectId">ID do objeto que será atacado.</param>
        public static void AttackTarget(long objectId)
        {
            AutoAttackMessage message = new AutoAttackMessage();
            message.ObjectId = objectId;
            message.AttackType = "strike";
            message.AttackStatus = true;
            AtavismClient.Instance.NetworkHelper.SendMessage(message);
        }

        /// <summary>
        /// Método para limpar o alvo.
        /// </summary>
        public static void ClearTarget()
        {
            _UpdateTarget(null);
        }

        /// <summary>
        /// Método para definir o alvo pelo nome.
        /// </summary>
        /// <param name="name">Nome do alvo.</param>
        public static void TargetByName(string name)
        {
            _UpdateTarget(ClientAPI.WorldManager.GetObjectNode(name));
        }

        // Esse método foi comentado e por isso não possui funcionalidade
        /*public static void TargetUnit(string unit)
        {
            //_UpdateTarget (MarsUnit._GetUnit (unit));
        }*/

        /// <summary>
        /// Método para definir o último inimigo como alvo.
        /// </summary>
        public static void TargetLastEnemy()
        {
            _UpdateTarget(_lastEnemy);
        }

        /// <summary>
        /// Método para definir o último alvo.
        /// </summary>
        public static void TargetLastTarget()
        {
            _UpdateTarget(_lastTarget);
        }

        /// <summary>
        /// Método para definir o inimigo mais próximo como alvo.
        /// </summary>
        /// <param name="reverse">Verdadeiro se desejar inverter a ordem de busca.</param>
        public static void TargetNearestEnemy(bool reverse)
        {
            List<long> worldObjOIDs = ClientAPI.WorldManager.GetObjectOidList();
            List<AtavismObjectNode> worldObjects = new List<AtavismObjectNode>();
            foreach (long worldObjOID in worldObjOIDs)
            {
                AtavismObjectNode worldObj = ClientAPI.WorldManager.GetObjectNode(worldObjOID);
                if (worldObj != null && worldObj.CheckBooleanProperty("attackable"))
                    worldObjects.Add(worldObj);
            }
            Vector3 playerPos = ClientAPI.GetPlayerObject().Position;
            // Aqui iria uma ordenação dos objetos de mundo com base na distância do jogador, mas está comentado

            AtavismObjectNode last = null;
            if (_currentTarget != null && _currentTarget.CheckBooleanProperty("attackable"))
                last = _currentTarget;
            else
                last = _lastEnemy;

            int index = -1;
            if (worldObjects.Contains(last))
                index = worldObjects.IndexOf(last);
            else
                last = null;

            if (last == null)
            {
                if (worldObjects.Count > 0)
                    _UpdateTarget(worldObjects[0]);
                return;
            }
            if (worldObjects.Count > index + 1)
                _UpdateTarget(worldObjects[index + 1]);
            else
                _UpdateTarget(worldObjects[0]);
        }

        /// <summary>
        ///        /// Método para definir o alvo por OID.
        /// </summary>
        /// <param name="oid">OID do alvo.</param>
        public static void TargetByOID(OID oid)
        {
            _UpdateTarget(ClientAPI.WorldManager.GetObjectNode(oid));
        }

        /// <summary>
        /// Método para obter o alvo atual.
        /// </summary>
        /// <returns>Retorna o alvo atual.</returns>
        public static AtavismObjectNode GetCurrentTarget()
        {
            return _currentTarget;
        }

        #endregion API de Alvo

        #region Campos

        // Informações sobre o alvo atual, o último alvo, o último inimigo e o objeto sob o mouse
        static AtavismObjectNode _currentTarget;
        static AtavismObjectNode _lastTarget;
        static AtavismObjectNode _lastEnemy;
        static AtavismObjectNode _mouseoverTarget;

        #endregion Campos

        #region Métodos auxiliares

        /// <summary>
        /// Método para atualizar o alvo.
        /// </summary>
        /// <param name="obj">Novo objeto alvo.</param>
        static void _UpdateTarget(AtavismObjectNode obj)
        {
            if (_currentTarget != obj)
            {
                if (_currentTarget != null)
                {
                    _lastTarget = _currentTarget;
                    if (_currentTarget.CheckBooleanProperty("attackable"))
                        _lastEnemy = _currentTarget;
                }
                _currentTarget = obj;
                // Aqui haveria um disparo de evento de mudança de alvo, mas está comentado.
            }
        }

        /// <summary>
        /// Método para lidar com a remoção de um objeto. Atualiza os campos conforme necessário.
        /// </summary>
        /// <param name="worldObj">Objeto a ser removido.</param>
        static void _HandleObjectRemoved(AtavismObjectNode worldObj)
        {
            if (_currentTarget == worldObj)
                _UpdateTarget(null);
            if (_lastTarget == worldObj)
                _lastTarget = null;
            if (_lastEnemy == worldObj)
                _lastEnemy = null;
            if (_mouseoverTarget == worldObj)
                _mouseoverTarget = null;
        }

        // Esse método foi comentado e por isso não possui funcionalidade
        /*void _UpdateMouseoverTarget (object sender, int eventData)
        {
            _mouseoverTarget = ClientAPI.GetMouseoverTarget ();
        }*/

        #endregion Métodos auxiliares

        // Registra para eventos de início de quadro, para que possamos atualizar nosso alvo de passagem do mouse
        //ClientAPI.RegisterEventHandler("FrameStarted", _UpdateMouseoverTarget);

        // Registra para mensagens de objeto removido, para que possamos limpar nosso alvo, se necessário
        //ClientAPI.World.RegisterEventHandler('ObjectRemoved', _HandleObjectRemoved);
    }
}
