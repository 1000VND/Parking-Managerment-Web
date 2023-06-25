import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DasboardService } from 'src/app/services/dasboard.service';
import { LoadingComponent } from '../common/loading/loading.component';
import { finalize } from 'rxjs';
import * as moment from 'moment';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;

  data: { month: string, value: number }[] = [];
  dataService: any[] = [];
  dataDoughnut: any[] = [];

  constructor(
    private _service: DasboardService,
    private _toastr: ToastrService,
  ) {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.getDataChart();
    this.getDataDoughnut();
    this.animation();
  }

  customizeLabelText(info: any) {
    return info.value != 0 ? `${info.value}` : ``
  };

  customizeTooltip(arg: any) {
    console.log(arg)
    return {
      text: `${arg.argumentText} : ${arg.valueText}`,
    };
  }

  customizeLabelDoughnut(point: any) {
    return `${point.argumentText}: ${point.valueText}%`;
  }

  getDataChart() {
    this.loading.loading(true);
    this._service.getDataChart().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      this.dataService = [...res.data];
      this.data = [
        {
          month: 'Month 1',
          value: this.dataService[0].month1
        },
        {
          month: 'Month 2',
          value: this.dataService[0].month2
        },
        {
          month: 'Month 3',
          value: this.dataService[0].month3
        },
        {
          month: 'Month 4',
          value: this.dataService[0].month4
        },
        {
          month: 'Month 5',
          value: this.dataService[0].month5
        },
        {
          month: 'Month 6',
          value: this.dataService[0].month6
        },
        {
          month: 'Month 7',
          value: this.dataService[0].month7
        },
        {
          month: 'Month 8',
          value: this.dataService[0].month8
        },
        {
          month: 'Month 9',
          value: this.dataService[0].month9
        },
        {
          month: 'Month 10',
          value: this.dataService[0].month10
        },
        {
          month: 'Month 11',
          value: this.dataService[0].month11
        },
        {
          month: 'Month 12',
          value: this.dataService[0].month12
        }
      ];
    })
  }

  getDataDoughnut() {
    this.loading.loading(true);
    this._service.getDataDoughnut().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      console.log(res.data)
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

  animation() {
    const valueDisplays = document.querySelectorAll(".num") as NodeListOf<HTMLElement>;
    const endValueDuration = 2000;

    valueDisplays.forEach((valueDisplay) => {
      let startValue = 0;
      let endValue = parseInt(valueDisplay.getAttribute("data-val")!);

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
    });
  }

}
