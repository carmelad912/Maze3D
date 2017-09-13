using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP.Search;
using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using ATP.MazeGenerators;
using ATP;
using System.Collections;
using ATP.Compression;
using System.IO.Compression;

namespace ATPProject.Model1
{

    class Model : IModel, IStoppable
    {
        /// <summary>
        /// The event of the model.
        /// </summary>
        public event ModelEventDelegate Event;

        /// <summary>
        /// Dictionary that holds all the maze and solutions.
        /// </summary>
        /// <remarks>the string - the name of the maze, list of objects - the object in the first place is the maze and in the second place is the solution.</remarks>
        public Dictionary<string, List<object>> m_mazesAndSolutins;

        /// <summary>
        /// List of all threads that run the generate method.
        /// </summary>
        public List<Thread> generate;

        /// <summary>
        /// List of all threads that run the solve method.
        /// </summary
        public List<Thread> solve;

        /// <summary>
        /// Number of working threads.
        /// </summary>
        int nuofthreads;

        /// <summary>
        /// The solving algorithm.
        /// </summary>
        string solvingalgo;

        /// <summary>
        /// The generating algorithm.
        /// </summary>
        string generatealgo;

        /// <summary>
        /// Number of completing threads.
        /// </summary>
        int compthreads;

        /// <summary>
        /// The constructor of the Model.
        /// </summary>
        /// <remarks>Activating Thread pool, then initialize the list of threads of generate and solve, initialize the dictionary
        /// of mazes and solutions. Check if we saved previously mazes and solutions, if true - load the proper file.</remarks>
        public Model()
        {
            ActivateThreadPool();
            generate = new List<Thread>();
            solve = new List<Thread>();
            m_mazesAndSolutins = new Dictionary<string, List<object>>();
            if (File.Exists("mazes_solutions.zip"))
                LoadDictionaryFromDisk("mazes_solutions.zip");
        }


        /// <summary>
        /// Activating thread pool.
        /// </summary>
        /// <remarks>Set the thread pool with the proper parameters.</remarks>
        private void ActivateThreadPool()
        {
            ThreadPool.SetMaxThreads(nuofthreads, compthreads);
        }

        /// <summary>
        /// Add Solution to the maze 'mazename' in the dictionary of mazes and solutions.
        /// </summary>
        /// <param name="mazename">The name of the maze we like to add his solution.</param>
        /// <param name="solution">The solution we like to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddSolution(string mazename, Solution solution)
        {
            m_mazesAndSolutins[mazename].Add(solution);
        }

        /// <summary>
        /// Get the solution of maze 'mazename'. 
        /// </summary>
        /// <remarks>Calls to the private method RetrieveSolution.</remarks>
        /// <param name="mazename">The maze for which we like to get solution.</param>
        /// <returns>The proper solution</returns>
        public Solution GetSolution(string mazename)
        {
            return RetrieveSolution(mazename);
        }

        /// <summary>
        /// Returns the solution of 'mazename' if exists.
        /// </summary>
        /// <remarks>If the maze 'mazename' is in the dictionary and if there is a solution for 'mazename', return the solution, otherwise return null.</remarks>
        /// <param name="mazename">The maze for which we like to get solution.</param>
        /// <returns>The proper solution if exists.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private Solution RetrieveSolution(string mazename)
        {
            if (m_mazesAndSolutins.ContainsKey(mazename))
            {
                if (m_mazesAndSolutins[mazename].Count > 1)
                    return (m_mazesAndSolutins[mazename].ElementAt(1) as Solution);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Add maze 'mazename' to the dictionary of mazes and solutions.
        /// </summary>
        /// <param name="mazename">The name of the maze we like to add.</param>
        /// <param name="solution">The Maze we like to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddMaze(string mazename, AMaze maze)
        {
            m_mazesAndSolutins[mazename] = new List<object>();
            m_mazesAndSolutins[mazename].Add(maze);
        }

        /// <summary>
        /// Get the maze 'mazename'. 
        /// </summary>
        /// <remarks>Calls to the private method RetrieveMaze.</remarks>
        /// <param name="mazename">The maze we like to get.</param>
        /// <returns>The proper maze.</returns>
        public AMaze GetMaze(string mazename)
        {
            return RetrieveMaze(mazename);
        }

        /// <summary>
        /// Returns maze 'mazename' if exists.
        /// </summary>
        /// <remarks>If the maze 'mazename' is in the dictionary, return the solution, otherwise return null and notify the user that the maze does not exist.</remarks>
        /// <param name="mazename">The maze we like to get.</param>
        /// <returns>The proper maze if exists.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private AMaze RetrieveMaze(string mazename)
        {
            if (m_mazesAndSolutins.ContainsKey(mazename))
                return (m_mazesAndSolutins[mazename].ElementAt(0) as AMaze);
            else
            {
                Event("MazeNotExist " + mazename);
                return null;
            }

        }

        /// <summary>
        /// Solve the maze 'mazename'.
        /// </summary>
        /// <remarks>If the maze exist and theres already solution for this maze notify the user - else solve the maze. if the maze
        /// does not exist, notify the user.</remarks>
        /// <param name="mazename">The name of the maze we want to solve.</param>
        public void SolveMaze(string mazename)
        {
            if (m_mazesAndSolutins.ContainsKey(mazename))
            {
                if (m_mazesAndSolutins[mazename].Count > 1)
                    Event("SolutionExist " + mazename);
                else
                    Solve(mazename);
            }
            else
                Event("MazeNotExist " + mazename);
        }

        /// <summary>
        /// Genereate maze and save his as mazename.
        /// </summary>
        /// <remarks>if a maze with the name 'mazename' already exist, notify the user, else generate the maze.</remarks>
        /// <param name="mazename">The name of the maze.</param>
        /// <param name="x">Number of rows.</param>
        /// <param name="y">Number of columns.</param>
        /// <param name="z">Number of floors.</param>
        public void GenerateMaze(string mazename, int x, int y, int z)
        {
            if (!m_mazesAndSolutins.ContainsKey(mazename.ToLower()))
                RunGenerate(mazename, x, y, z);
            else
                Event("MazeExist " + mazename);
        }

        /// <summary>
        /// Solve the maze 'mazename'.
        /// </summary>
        /// <remarks>Check if there is a maze that is not 'mazename' that equals to 'mazename', if true - add his solution to be 'mazename' solution.
        /// else solve in thread - if solvingalgo is 'bfs' solve with bfs, if solvingalgo is 'dfs' solve with bfs. add the solution and if
        /// exists notify the user.</remarks>
        /// <param name="mazename">The maze we want to solve.</param>
        private void Solve(string mazename)
        {
            foreach (string key in m_mazesAndSolutins.Keys)
            {
                if (!key.Equals(mazename))
                {
                    if ((m_mazesAndSolutins[key].ElementAt(0) as AMaze).Equals((m_mazesAndSolutins[mazename].ElementAt(0) as AMaze)) && m_mazesAndSolutins[key].Count > 1)
                    {
                        m_mazesAndSolutins[mazename].Add(m_mazesAndSolutins[key].ElementAt(1));
                        return;
                    }
                }
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                SearchableMaze3d sm = new SearchableMaze3d((m_mazesAndSolutins[mazename].ElementAt(0) as Maze3d));
                Solution sol;
                if (solvingalgo.ToLower().Equals("dfs"))
                {
                    DepthFirstSearch ds = new DepthFirstSearch();
                    sol = ds.Solve(sm);
                }
                else
                {
                    BreadthFirstSearch bs = new BreadthFirstSearch();
                    sol = bs.Solve(sm);
                }
                if (sol != null)
                {
                    AddSolution(mazename, sol);
                    Event("SolutionReady " + mazename);
                }
            }));
        }

        /// <summary>
        /// Generate new maze.
        /// </summary>
        /// <remarks>run in new thread and generate a maze 'mazename' with x,y,z, add the maze to the dictionary and notify the user that
        /// the maze is ready.</remarks>
        /// <param name="mazename">The name of the maze.</param>
        /// <param name="x">Number of rows.</param>
        /// <param name="y">Number of columns.</param>
        /// <param name="z">Number of floors.</param>
        private void RunGenerate(string mazename, int x, int y, int z)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                ArrayList s = new ArrayList();
                s.Add(x);
                s.Add(y);
                s.Add(z);
                MyMaze3dGenerator mg = new MyMaze3dGenerator();
                AMaze maze = mg.generate(s);
                if (maze != null)
                {
                    AddMaze(mazename, maze);

                    Event("MazeReady " + mazename);
                }
            })
                );
        }

        /// <summary>
        /// Print soluiton.
        /// </summary>
        /// <remarks>Get the solution from the dictionary and turn it to a string, then return it.</remarks>
        /// <param name="mazename">The maze we want to print its solution.</param>
        /// <returns>The solution.</returns>
        public string printsolution(string mazename)
        {
            Solution sol = GetSolution(mazename);
            string solution = "";
            foreach (Astate position in sol.m_solu)
            {
                solution = solution + position.state + " \n";
            }
            return solution;
        }

        /// <summary>
        /// Load the dictionary of mazes and solutions from disk.
        /// </summary>
        /// <param name="path">The path where the dictionary exist.</param>
        public void LoadDictionaryFromDisk(string path)
        {
            GZipStream zip = new GZipStream(File.OpenRead(path), CompressionMode.Decompress);
            try
            {
                m_mazesAndSolutins = (Dictionary<string, List<object>>)(new BinaryFormatter()).Deserialize(zip);
            }
            catch (Exception e)
            {
                Event("UnzipError  " + e);
            }
            finally
            {
                if (zip != null)
                    ((IDisposable)zip).Dispose();
            }
        }

        /// <summary>
        /// Save the dictionary of mazes and solutions to disk.
        /// </summary>
        /// <param name="path">The path where we want to save the dictionary.</param>
        public void SaveDictionaryToDisk(string path)
        {
            byte[] tocompress;
            MemoryStream memorystram = new MemoryStream();
            try
            {
                GZipStream zip = new GZipStream(memorystram, CompressionMode.Compress);
                try
                {
                    (new BinaryFormatter()).Serialize(zip, m_mazesAndSolutins);
                }
                catch
                {
                    Event("ZipError  ");
                }
                finally
                {
                    if (zip != null)
                        ((IDisposable)zip).Dispose();
                }
                tocompress = memorystram.ToArray();
            }
            finally
            {
                if (memorystram != null)
                    ((IDisposable)memorystram).Dispose();
            }
            File.WriteAllBytes(path + ".zip", tocompress);
        }

        /// <summary>
        /// Compress a maze 'mazename' from 'mazes' and save it in a file in the given path.
        /// </summary>
        /// <remarks>1. Check if the dictionary mazes and solutions contains a maze with the key mazename. If true,
        /// turn the maze 'mazename' in to bytearray else notifiy the user that such maze does not exist.
        /// 2.open a new file and call to outStream.Write, for writing in to the file, we move from 0 in packets of 100 (the size of the buffer in MyCompressorStream)
        /// until we get to the end of the array of bytes comp. notify the user the maze saved.</remarks>
        /// <param name="mazename">The maze we would like to save in to a file.</param>
        /// <param name="path">The path given for saving there our maze.</param>
        public void SaveMaze(string mazename, string path)
        {
            if (m_mazesAndSolutins.ContainsKey(mazename.ToLower()))
            {
                byte[] comp = (m_mazesAndSolutins[mazename.ToLower()].ElementAt(0) as Maze3d).toByteArray();
                using (FileStream fileOutStream = new FileStream(path, FileMode.Create))
                {
                    using (Stream outStream = new MyCompressorStream(fileOutStream, 1))
                    {
                        int current = 0;
                        while (current < comp.Length)
                        {
                            if (current + 100 < comp.Length)
                                outStream.Write(comp, current, 100);
                            else
                                outStream.Write(comp, current, comp.Length - current);
                            current = current + 100;
                        }
                        outStream.Flush();
                    }
                }
            }
            Event("MazeSaved " + mazename);
        }

        /// <summary>
        /// Load a maze from a file in the path given and saves it as a new maze in 'mazes'.
        /// </summary>
        /// <remarks>1. Check if the contains a maze with the key mazename. If true,
        /// delete the old maze from mazes and from dictionary solutions of the controller, if exists.
        /// 2. Read all the bytes in the path given and from the array that returned get the sizes of the maze in the directory by rhe function getMazeSizes.
        /// 3. create a new byte array in a size of the sizes we got from getMazeSizes.
        /// 4. open the file for reading and call to outStream.Read - we move from 0 in packets of 100 (the size of the buffer in MyCompressorStream)
        /// until we get to the end of the array of bytes 'buffer'.
        /// 5. generate new maze by the constructor maze3d(byte[]) and send 'buffer' to the constructer, add the maze to 'mazes'.</remarks>
        /// <param name="path">The path of the compressed maze.</param>
        /// <param name="mazename">The name of the maze we will save.</param>
        public void LoadMaze(string mazename, string path)
        {
            byte[] todecompress = File.ReadAllBytes(path);
            int[] sizes = getMazeSizes(todecompress);
            byte[] buffer = new byte[sizes[0] * sizes[1] * sizes[2] + 3];
            using (FileStream fileOutStream = new FileStream(path, FileMode.Open)){
                using (Stream outStream = new MyCompressorStream(fileOutStream, 2)){
                    int current = 0;
                    while (current < buffer.Length){
                        if (current + 100 < buffer.Length)
                            outStream.Read(buffer, current, 100);
                        else
                            outStream.Read(buffer, current, buffer.Length - current);
                        current = current + 100;
                    }
                    outStream.Flush();
                }
            }
            Maze3d maze = new Maze3d(buffer);
            if (!m_mazesAndSolutins.ContainsKey(mazename)){
                ArrayList s = new ArrayList();
                s.Add(sizes[0]);
                s.Add(sizes[1]);
                s.Add(sizes[2]-2);
                Maze3d newmaze= new Maze3d(s);
                fixLoad(mazename, newmaze, maze);
            }
            if (maze != null)
                Event("DisplayMaze " + mazename);
            else
                Event("DisplayError " + mazename);
        }

        /// <summary>
        /// Private function to fix the loaded maze.
        /// </summary>
        /// <param name="mazename">The name of the maze</param>
        /// <param name="newmaze">maze we will save</param>
        /// <param name="maze">maze that loaded</param>
        private void fixLoad(string mazename, AMaze newmaze, AMaze maze)
        {
            for (int i = 0; i < (int)maze.sizes[2] - 1; i++)
            {
                for (int j = 0; j < (int)maze.sizes[0]; j++)
                {
                    for (int k = 0; k < (int)maze.sizes[1]; k++)
                    {
                        (newmaze as Maze3d).maze3d[i].maze2d[j, k] = (maze as Maze3d).maze3d[i].maze2d[j, k];
                    }
                }
            }
            newmaze.start = maze.start;
            newmaze.end = maze.end;
            m_mazesAndSolutins[mazename] = new List<object>();
            m_mazesAndSolutins[mazename].Add(newmaze);
        }

        /// <summary>
        /// Get byte array and find the number of rows, columns and floor of a maze.
        /// </summary>
        /// <remarks>1. Build an int array in a size of 3 - for x,y and z.
        /// 2. if the element in the second place in the array the function got is 0 - according the compress function that means 
        /// that the element in the first place appears once, for example the sizes 4, 5, 5.
        /// 2.1 first place in return will be the first place in toreturn.
        /// 2.2 if the element in the 4th place is 0 - all the sizes are different for example 4,5,6.
        /// 2.2.1 second place in toreturn will be the third place in bytearray, and third place in toreturn  will be the 5th place in bytearray.
        /// 2.3 else the number of columns and floors equals - for example 4,5,5 - second and third place in toreturn will be the third place in bytearray.
        /// 3. else if if the element in the second place in the array the function got is 1 - number of rows and columns is equal - for example 4,4,5
        /// 3.1 first and second place in toreturn will be the second place in bytearray and the third place in toreturn will be the third place in bytearray.
        /// 4.else if if the element in the second place in the array the function got is 2 - the number of rows, columns and floores are the same - for example 5,5,5.
        /// 4.1 firs, second and third places in toreturn will be the second place in bytearray.
        /// </remarks>
        /// <param name="bytearray">The array of bytes from which we get the sizes of the maze we like to decompress.</param>
        /// <returns>Int array with the sizes of the maze.</returns>
        private int[] getMazeSizes(byte[] bytearray)
        {
            int[] toreturn = new int[3];
            if (bytearray[1] == 0)
            {
                toreturn[0] = bytearray[0];
                if (bytearray[3] == 0)
                {
                    toreturn[1] = bytearray[2];
                    toreturn[2] = bytearray[4];
                }
                else
                    toreturn[1] = toreturn[2] = bytearray[2];
            }
            else if (bytearray[1] == 1)
            {
                toreturn[0] = toreturn[1] = bytearray[0];
                toreturn[2] = bytearray[2];
            }
            else if (bytearray[1] == 2)
                toreturn[0] = toreturn[1] = toreturn[2] = bytearray[0];
            return toreturn;
        }

        /// <summary>
        /// Stop all threads running of generate and solve.
        /// </summary>
        public void Stop()
        {
            SaveDictionaryToDisk("mazes_solutions");
            foreach (Thread gen in generate)
            {
                gen.Abort();
            }
            foreach (Thread solver in solve)
            {
                solver.Abort();
            }
        }

        /// <summary>
        /// Set the properties as given.
        /// </summary>
        /// <param name="numoft">Number of working threads.</param>
        /// <param name="numofct">Number of completing threads.</param>
        /// <param name="solvingalgorithm">The solving algorithm.</param>
        /// <param name="generatealgo">The generate algorithm.</param>
        public void SetProp(int numoft, int numofct, string solvingalgorithm, string generatealgo)
        {
            nuofthreads = numoft;
            solvingalgo = solvingalgorithm;
            this.generatealgo = generatealgo;
            compthreads = numofct;
        }
    }

}
