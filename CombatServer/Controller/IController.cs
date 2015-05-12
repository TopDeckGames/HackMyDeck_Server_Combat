using System;
using System.IO;

namespace GestionServer
{
    public interface IController
    {
        Response parser(Stream stream);
    }
}