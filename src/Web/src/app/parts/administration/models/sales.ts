import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import Debug from 'debug';

import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import {
  ChartType as ChartTypeBase,
  ChartModel as ChartModelBase,
  ByStateType as ByStateTypeBase,
  ByStateModel as ByStateModelBase,
  WeekOverWeekType as WeekOverWeekTypeBase,
  WeekOverWeekModel as WeekOverWeekModelBase
} from '../../../models/ordering/sales';

const debug = new Debug('sales');

export interface ChartType extends ChartTypeBase {

}
export const ChartModel = ChartModelBase;

export interface ByStateType extends ByStateTypeBase {

}
export const ByStateModel = ByStateModelBase;

export interface WeekOverWeekType extends WeekOverWeekTypeBase {

}
export const WeekOverWeekModel = WeekOverWeekModelBase;
