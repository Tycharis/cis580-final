using System;
using Microsoft.Xna.Framework;

namespace CIS580_Final
{
    internal class MathHandler
    {
        // Math properties for buildings
        internal static double BtcPerCpu => 0.1 * BtcPerGpu;
        internal static double BtcPerGpu { get; set; }
        internal static double BtcPerServer => 8 * BtcPerGpu;
        internal static double BtcPerMiner => 47 * BtcPerGpu;
        internal static double BtcPerSupercomputer => 260 * BtcPerGpu;

        // Statistics
        internal double Bitcoin { get; private set; }
        internal double Bps;
        internal double Usd => Bitcoin / BtcPerGpu;

        // Building counters
        internal int NumberOfCpus;
        internal int NumberOfGpus;
        internal int NumberOfMiners;
        internal int NumberOfServers;
        internal int NumberOfSupercomputers;

        // Base costs
        private static double CpuBaseCost => 15 * BtcPerGpu;
        private static double GpuBaseCost => 100 * BtcPerGpu;
        private static double ServerBaseCost => 1100 * BtcPerGpu;
        private static double MinerBaseCost => 12000 * BtcPerGpu;
        private static double SupercomputerBaseCost => 130000 * BtcPerGpu;

        // Building costs
        internal double CpuCost => CpuBaseCost  * Math.Pow(1.15, NumberOfCpus);
        internal double GpuCost => GpuBaseCost * Math.Pow(1.15, NumberOfGpus);
        internal double ServerCost => ServerBaseCost * Math.Pow(1.15, NumberOfServers);
        internal double MinerCost => MinerBaseCost * Math.Pow(1.15, NumberOfMiners);
        internal double SupercomputerCost => SupercomputerBaseCost * Math.Pow(1.15, NumberOfSupercomputers);

        // Upgrade counters
        private int _cpuUpgrades;
        private int _gpuUpgrades;
        private int _serverUpgrades;
        private int _minerUpgrades;
        private int _supercomputerUpgrades;

        // Upgrade costs
        internal double CpuUpgradeCost => CpuBaseCost * Math.Pow(10, _cpuUpgrades);
        internal double GpuUpgradeCost => GpuBaseCost * Math.Pow(10, _gpuUpgrades);
        internal double ServerUpgradeCost => ServerBaseCost * Math.Pow(10, _serverUpgrades);
        internal double MinerUpgradeCost => MinerBaseCost * Math.Pow(10, _minerUpgrades);
        internal double SupercomputerUpgradeCost => SupercomputerBaseCost * Math.Pow(10, _supercomputerUpgrades);

        internal void Click()
        {
            Bitcoin += BtcPerGpu;
        }

        internal void TryBuyBuilding(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Cpu:
                    if (Bitcoin >= CpuCost)
                    {
                        NumberOfCpus++;
                        Bitcoin -= CpuCost;
                    }

                    break;
                case BuildingType.Gpu:
                    if (Bitcoin >= GpuCost)
                    {
                        NumberOfGpus++;
                        Bitcoin -= GpuCost;
                    }

                    break;
                case BuildingType.Server:
                    if (Bitcoin >= ServerCost)
                    {
                        NumberOfServers++;
                        Bitcoin -= ServerCost;
                    }

                    break;
                case BuildingType.Miner:
                    if (Bitcoin >= MinerCost)
                    {
                        NumberOfMiners++;
                        Bitcoin -= MinerCost;
                    }

                    break;
                case BuildingType.Supercomputer:
                    if (Bitcoin >= SupercomputerCost)
                    {
                        NumberOfSupercomputers++;
                        Bitcoin -= SupercomputerCost;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        internal void TryBuyUpgrade(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Cpu:
                    if (Bitcoin >= CpuUpgradeCost)
                    {
                        _cpuUpgrades++;
                    }

                    break;
                case BuildingType.Gpu:
                    if (Bitcoin >= GpuUpgradeCost)
                    {
                        _gpuUpgrades++;
                    }

                    break;
                case BuildingType.Server:
                    if (Bitcoin >= ServerUpgradeCost)
                    {
                        _serverUpgrades++;
                    }

                    break;
                case BuildingType.Miner:
                    if (Bitcoin >= MinerUpgradeCost)
                    {
                        _minerUpgrades++;
                    }

                    break;
                case BuildingType.Supercomputer:
                    if (Bitcoin >= SupercomputerUpgradeCost)
                    {
                        _supercomputerUpgrades++;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        internal void Initialize()
        {
            BtcPerGpu = WebHandler.GetConversionFactor();
        }

        internal void Update(GameTime gameTime)
        {
            double bps = 0.0d;

            bps += NumberOfCpus * BtcPerCpu * (2 * _cpuUpgrades == 0 ? 1 : 2 * _cpuUpgrades);
            bps += NumberOfGpus * BtcPerGpu * (2 * _gpuUpgrades == 0 ? 1 : 2 * _gpuUpgrades);
            bps += NumberOfServers * BtcPerServer * (2 * _serverUpgrades == 0 ? 1 : 2 * _serverUpgrades);
            bps += NumberOfMiners * BtcPerMiner * (2 * _minerUpgrades == 0 ? 1 : 2 * _serverUpgrades);
            bps += NumberOfSupercomputers * BtcPerSupercomputer * (2 * _supercomputerUpgrades == 0 ? 1 : 2 * _supercomputerUpgrades);

            Bps = bps;

            Bitcoin += bps * gameTime.ElapsedGameTime.TotalSeconds;

            // Ain't rounding errors fun?
            if (Bitcoin < 0.0d)
            {
                Bitcoin = 0.0d;
            }
        }
    }
}
