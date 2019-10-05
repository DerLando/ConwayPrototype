using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry;
using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Parsing
{
    public class Operator
    {
        private PlanktonMesh _pMesh;

        public Operator(Mesh mesh)
        {
            _pMesh = mesh.ToPlanktonMeshWithNgons();
        }

        public Operator(PlanktonMesh pMesh)
        {
            _pMesh = pMesh;
        }

        private void _applyOperation(Token token)
        {
            switch (token.Operation)
            {
                case Operation.kis:
                    _pMesh = _pMesh.Kis();
                    break;
                case Operation.ambo:
                    _pMesh = _pMesh.Ambo();
                    break;
                case Operation.gyro:
                    break;
                case Operation.dual:
                    _pMesh = _pMesh.Dual();
                    break;
                case Operation.reflect:
                    break;
                case Operation.expand:
                    _pMesh = _pMesh.Expand();
                    break;
                case Operation.bevel:
                    _pMesh = _pMesh.Bevel();
                    break;
                case Operation.ortho:
                    _pMesh = _pMesh.Ortho();
                    break;
                case Operation.meta:
                    _pMesh = _pMesh.Meta();
                    break;
                case Operation.truncate:
                    _pMesh = _pMesh.Truncate();
                    break;
                case Operation.@join:
                    _pMesh = _pMesh.Join();
                    break;
                case Operation.split:
                    break;
                case Operation.propellor:
                    break;
                case Operation.flatten:
                    break;
                case Operation.loft:
                    _pMesh = _pMesh.Loft();
                    break;
                case Operation.needle:
                    _pMesh = _pMesh.Needle();
                    break;
                case Operation.zip:
                    _pMesh = _pMesh.Zip();
                    break;
                case Operation.quinto:
                    _pMesh = _pMesh.Quinto();
                    break;
                case Operation.none:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Mesh Operate(IEnumerable<Token> token)
        {
            // operate all operations
            foreach (var tok in token)
            {
                _applyOperation(tok);
            }

            return _pMesh.ToRhinoMeshWithNgons();
        }

        public Mesh GetMesh()
        {
            return _pMesh.ToRhinoMeshWithNgons();
        }
    }
}
