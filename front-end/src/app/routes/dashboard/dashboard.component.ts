import { Component, OnInit, ViewChild } from '@angular/core';
import { DasboardService } from 'src/app/services/dasboard.service';
import { LoadingComponent } from '../common/loading/loading.component';
import { finalize } from 'rxjs';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;

  data: { month: string, value: number, valueLine: number }[] = [];
  dataService: any[] = [];
  dataDoughnut: any[] = [];
  dataCard: any;
  card1: number = 0;
  card2: number = 0;
  card3: number = 0;
  card4: number = 0;
  [key: string]: any;
  constructor(
    private _service: DasboardService,
  ) {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.getDataChart();
    this.getDataDoughnut();
    this.getDataCard();
  }

  customizeLabelText(info: any) {
    return info.value != 0 ? `${info.value}` : ``
  };

  customizeTooltip(arg: any) {
    return {
      text: `${arg.argumentText} : ${arg.valueText}`,
    };
  }

  customizeTooltipPie(arg: any) {
    return {
      text: `${arg.argumentText} : ${arg.valueText}%`,
    };
  }

  customizeLabelDoughnut(point: any) {
    return `${point.argumentText}: ${point.valueText}%`;
  }

  getDataChart() {
    this.loading.loading(true);
    this._service.countAllTicketMonthly().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      this.dataService = [...res.data];
      this.data = [
        {
          month: 'Month 1',
          value: this.dataService[0].month1,
          valueLine: this.dataService[0].carReturnMonth1
        },
        {
          month: 'Month 2',
          value: this.dataService[0].month2,
          valueLine: this.dataService[0].carReturnMonth2
        },
        {
          month: 'Month 3',
          value: this.dataService[0].month3,
          valueLine: this.dataService[0].carReturnMonth3
        },
        {
          month: 'Month 4',
          value: this.dataService[0].month4,
          valueLine: this.dataService[0].carReturnMonth4
        },
        {
          month: 'Month 5',
          value: this.dataService[0].month5,
          valueLine: this.dataService[0].carReturnMonth5
        },
        {
          month: 'Month 6',
          value: this.dataService[0].month6,
          valueLine: this.dataService[0].carReturnMonth6
        },
        {
          month: 'Month 7',
          value: this.dataService[0].month7,
          valueLine: this.dataService[0].carReturnMonth7
        },
        {
          month: 'Month 8',
          value: this.dataService[0].month8,
          valueLine: this.dataService[0].carReturnMonth8
        },
        {
          month: 'Month 9',
          value: this.dataService[0].month9,
          valueLine: this.dataService[0].carReturnMonth9
        },
        {
          month: 'Month 10',
          value: this.dataService[0].month10,
          valueLine: this.dataService[0].carReturnMonth10
        },
        {
          month: 'Month 11',
          value: this.dataService[0].month11,
          valueLine: this.dataService[0].carReturnMonth11
        },
        {
          month: 'Month 12',
          value: this.dataService[0].month12,
          valueLine: this.dataService[0].carReturnMonth12
        }
      ];
    })
  }

  getDataDoughnut() {
    this.loading.loading(true);
    this._service.rateMaleFemale().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      this.dataDoughnut = [
        {
          type: 'Male',
          percent: res.data.percentMale
        },
        {
          type: 'Female',
          percent: res.data.percentFemale
        }
      ]
    })
  }

  getDataCard() {
    this.loading.loading(true);
    this._service.getDataCard().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      this.card1 = res.data[0].carNumber;
      this.card2 = 100 - res.data[0].carNumber;
      this.card3 = res.data[0].carToday;
      this.card4 = res.data[0].moneyToday;
      this.animation();
    })
  }

  animation() {
    const valueDisplays = document.querySelectorAll(".num") as NodeListOf<HTMLElement>;
    const endValueDuration = 2000;

    valueDisplays.forEach((valueDisplay, index) => {
      const propertyName = `card${index + 1}`;
      if (propertyName == 'card4') {
        const endValue = this[propertyName];
        let startValue = 0;

        let updateInterval = endValueDuration / endValue;

        let counter = setInterval(() => {
          let increment = Math.ceil(Math.random() * 10000);
          startValue += increment;

          if (startValue >= endValue) {
            startValue = endValue;
            clearInterval(counter);
          }

          valueDisplay.textContent = startValue.toString();
        }, updateInterval);

      } else {
        const endValue = this[propertyName];
        let startValue = 0;

        let updateInterval = endValueDuration / endValue;

        let counter = setInterval(() => {
          let increment = Math.ceil(Math.random() * 50);
          startValue += increment;

          if (startValue >= endValue) {
            startValue = endValue;
            clearInterval(counter);
          }

          valueDisplay.textContent = startValue.toString();
        }, updateInterval);
      }
    });
  }
}

