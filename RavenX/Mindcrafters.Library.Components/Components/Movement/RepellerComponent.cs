#region File description
//------------------------------------------------------------------------------
//RepellerComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft
#endregion

#region GarageGames

using GarageGames.Torque.Core;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary> 
    ///A component to add a push force to objects in its effect radius
    ///Note: implemented as a negative attractor
    ///</summary> 
    [TorqueXmlSchemaType]
    public class RepellerComponent : AttractorComponent
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///The strength of repulsion
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "200")]
        new public float Strength
        {
            get { return -1 * _strength; }
            set { _strength = -1 * value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            RepellerComponent obj2 = obj as RepellerComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //marked with the attribute [XmlIgnore]
            //obj2.Property = Property;
            obj2.Strength = Strength;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }
}
