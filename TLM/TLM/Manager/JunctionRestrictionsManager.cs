﻿using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TrafficManager.Geometry;
using TrafficManager.State;
using TrafficManager.Traffic;

namespace TrafficManager.Manager {
	public class JunctionRestrictionsManager {
		public static JunctionRestrictionsManager Instance { get; private set; } = null;

		static JunctionRestrictionsManager() {
			Instance = new JunctionRestrictionsManager();
		}

		internal bool HasJunctionRestrictions(ushort nodeId) {
			NetManager netManager = Singleton<NetManager>.instance;

			if ((netManager.m_nodes.m_buffer[nodeId].m_flags & NetNode.Flags.Created) == NetNode.Flags.None)
				return false;

			for (int i = 0; i < 8; ++i) {
				ushort segmentId = netManager.m_nodes.m_buffer[nodeId].GetSegment(i);
				if (segmentId == 0)
					continue;

				Configuration.SegmentNodeFlags flags = Flags.getSegmentNodeFlags(segmentId, netManager.m_segments.m_buffer[segmentId].m_startNode == nodeId);
				if (flags != null && !flags.IsDefault())
					return true;
			}
			return false;
		}

		internal bool IsLaneChangingAllowedWhenGoingStraight(ushort segmentId, bool startNode) {
			return Flags.getStraightLaneChangingAllowed(segmentId, startNode);
		}

		internal bool IsUturnAllowed(ushort segmentId, bool startNode) {
			return Flags.getUTurnAllowed(segmentId, startNode);
		}

		internal bool IsEnteringBlockedJunctionAllowed(ushort segmentId, bool startNode) {
			return Flags.getEnterWhenBlockedAllowed(segmentId, startNode);
		}

		internal bool IsPedestrianCrossingAllowed(ushort segmentId, bool startNode) {
			return Flags.getPedestrianCrossingAllowed(segmentId, startNode);
		}

		internal void ToggleLaneChangingAllowedWhenGoingStraight(ushort segmentId, bool startNode) {
			Flags.setStraightLaneChangingAllowed(segmentId, startNode, !IsLaneChangingAllowedWhenGoingStraight(segmentId, startNode));
		}

		internal void ToggleUturnAllowed(ushort segmentId, bool startNode) {
			Flags.setUTurnAllowed(segmentId, startNode, !IsUturnAllowed(segmentId, startNode));
		}

		internal void ToggleEnteringBlockedJunctionAllowed(ushort segmentId, bool startNode) {
			Flags.setEnterWhenBlockedAllowed(segmentId, startNode, !IsEnteringBlockedJunctionAllowed(segmentId, startNode));
		}

		internal void TogglePedestrianCrossingAllowed(ushort segmentId, bool startNode) {
			Flags.setPedestrianCrossingAllowed(segmentId, startNode, !IsPedestrianCrossingAllowed(segmentId, startNode));
		}
	}
}
