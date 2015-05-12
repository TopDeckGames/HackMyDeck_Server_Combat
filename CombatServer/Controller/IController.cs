using System;
using System.IO;

namespace CombatServer
{
    public interface IController
    {
        Response parser(Stream stream);
    }
}