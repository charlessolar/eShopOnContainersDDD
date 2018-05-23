import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import Debug from 'debug';

import { sort } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { ChartType, ChartModel, WeekOverWeekType, WeekOverWeekModel, ByStateType, ByStateModel } from '../models/sales';

const debug = new Debug('dashboard');

export interface DashboardStoreType {
  loading: boolean;

  chart: Map<string, ChartType>;
  weekOverWeek: Map<string, WeekOverWeekType>;
  byState: Map<string, ByStateType>;

  period: { from: string, to: string };

  readonly chartData: { label: string, value: number}[];
  readonly weekOverWeekData: { label: string, value: number}[];
  readonly byStateData: { label: string, value: number}[];
  get: () => Promise<{}>;
}

export const DashboardStoreModel = types
  .model('DashboardStore', {
    loading: types.optional(types.boolean, false),

    chart: types.optional(types.map(ChartModel), {}),
    weekOverWeek: types.optional(types.map(WeekOverWeekModel), {}),
    byState: types.optional(types.map(ByStateModel), {}),

    period: types.maybe(types.model({
      from: types.string,
      to: types.string
    })),
  })
  .views(self => ({
    get chartData() {
      return sort(Array.from(self.chart.values()), 'label', 'desc').map(x => {
        const date = DateTime.fromISO(x.label);

        return {
          label: date.toFormat('MMM d'),
          value: x.value / 100
        };
      });
    },
    get weekOverWeekData() {
      const reduced = Array.from(self.weekOverWeek.values()).reduce((prev, cur) => { prev[cur.dayOfWeek] = { day: cur.dayOfWeek, value: cur.value / 100 }; return prev; }, {});
      return [
        reduced['Sunday'],
        reduced['Monday'],
        reduced['Tuesday'],
        reduced['Wednesday'],
        reduced['Thursday'],
        reduced['Friday'],
        reduced['Saturday']
      ];
    },
    get byStateData() {
      return Array.from(self.byState.values()).reduce((prev, cur) => { prev['US-' + cur.state] = cur.value / 100; return prev; }, {});
    }
  }))
  .actions(self => {
    const get = flow(function*() {

      const requestChart = new DTOs.OrderingSalesChart();
      const requestWeekOverWeek = new DTOs.OrderingSalesWeekOverWeek();
      const requestByState = new DTOs.OrderingSalesByState();

      if (self.period && self.period.from) {
        requestChart.from = self.period.from;
        requestWeekOverWeek.from = self.period.from;
        requestByState.from = self.period.from;
      }
      if (self.period && self.period.to) {
        requestChart.to = self.period.to;
        requestWeekOverWeek.to = self.period.to;
        requestByState.to = self.period.to;
      }

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;

        const results: [DTOs.PagedResponse<DTOs.SalesChart>, DTOs.PagedResponse<DTOs.SalesWeekOverWeek>, DTOs.PagedResponse<DTOs.SalesByState>] =
          yield Promise.all([
            client.paged(requestChart),
            client.paged(requestWeekOverWeek),
            client.paged(requestByState)
          ]);

        self.chart.replace(results[0].records.map(x => [x.id, x]));
        self.weekOverWeek.replace(results[1].records.map(x => [x.id, x]));
        self.byState.replace(results[2].records.map(x => [x.id, x]));
       } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });

    return { get };
  });
