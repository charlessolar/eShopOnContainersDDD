import Debug from 'debug';
import { ByStateModel as ByStateModelBase, ByStateType as ByStateTypeBase, ChartModel as ChartModelBase, ChartType as ChartTypeBase, WeekOverWeekModel as WeekOverWeekModelBase, WeekOverWeekType as WeekOverWeekTypeBase } from '../../../models/ordering/sales';

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
