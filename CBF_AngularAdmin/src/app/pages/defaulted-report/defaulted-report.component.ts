import { Component, OnInit } from '@angular/core';
import { ReportService } from '../pick-report/report.service';
import { Config } from 'src/app/utility/config';
import { AngularCsv } from 'angular-csv-ext/dist/Angular-csv';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-defaulted-report',
  templateUrl: './defaulted-report.component.html',
  styleUrls: ['./defaulted-report.component.scss']
})
export class DefaultedReportComponent implements OnInit {
  Weeks: any;
  poolId: any;
  show: boolean;
  Reports: any;
  menus: any;
  scheduleList: any;
  pooName: any;
  weekNumber: any;
  currentWeek: any;
  filteredItems: any;
  showTicketTextbox: boolean;
  showPoolTextbox: boolean;
  showFirstTextbox: boolean;
  showLastTextbox: boolean;
 showEmailTextbox: boolean;
  filterData: any = {
    isFilter: false,
    isSorting: false,
    isAscending: true,
    shortByName: "First_Name",
    filterByName: "None",
    filterByValue: ""
  };
  constructor(
    private service: ReportService,
    private config: Config
  ) { }

  ngOnInit() {
    this.getReport();
  }

  weekChange(newPage: number) {
    ({
      queryParams: { page: newPage }
    });
  }
 
  getReport() {
    this.config.startLoader();
    this.service.GetDefaultedReport().subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.Reports = res.defaulted_Report;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  exportTOCSV() {
    var options = {
      fieldSeparator: ",",
      quoteStrings: '"',
      decimalseparator: ".",
      noDownload: false,
      headers: [
        "Entry Name",
        "Pool Name",
        "First Name",
        "Last Name",
        "Email Address",
      ],
      nullToEmptyString: true
    };
    var PickReports = [];
    this.Reports.forEach(objReport => {
      let pickReport = {
        entryName: objReport.entryName,
        pool_Name: objReport.pool_Name,
        first_Name: objReport.first_Name,
        last_Name: objReport.last_Name,
        email_Address: objReport.email_Address,
      };
      PickReports.push(pickReport);
    });
    new AngularCsv(PickReports, "Defaulted Report", options);
  }
  sortDefaultedReportBy(shortByName) {
    this.filterData.isAscending = !this.filterData.isAscending;
    this.filterData.isSorting = true;
    this.filterData.shortByName = shortByName;
    switch (shortByName) {
      case "entryName": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.entryName.localeCompare(b.entryName));
        } else {
          this.Reports.sort((a, b) => b.entryName.localeCompare(a.entryName));
        }
        break;
      }
      case "pool_Name": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.pool_Name.localeCompare(b.pool_Name));
        } else {
          this.Reports.sort((a, b) => b.pool_Name.localeCompare(a.pool_Name));
        }
        break;
      }
      case "first_Name": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.first_Name.localeCompare(b.first_Name));
        } else {
          this.Reports.sort((a, b) => b.first_Name.localeCompare(a.first_Name));
        }
        break;
      }
      case "last_Name": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.last_Name.localeCompare(b.last_Name));
        } else {
          this.Reports.sort((a, b) => b.last_Name.localeCompare(a.last_Name));
        }
        break;
      }
      case "email_Address": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.email_Address.localeCompare(b.email_Address));
        } else {
          this.Reports.sort((a, b) => b.email_Address.localeCompare(a.email_Address));
        }
        break;
      }
    }
  }
  filterByTicket(value) {
    if (!value) {
      this.getReport();
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.entryName.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Reports = this.filteredItems;
  }

  filterByPool(value) {
    if (!value) {
      this.getReport();
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.pool_Name.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Reports = this.filteredItems;
  }
 
  filterByFirst(value) {
    if (!value) {
      this.getReport();
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.first_Name.indexOf(value) > -1
    );
    this.Reports = this.filteredItems;
  }
  filterByLast(value) {
    if (!value) {
      this.getReport();
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.last_Name.indexOf(value) > -1
    );
    this.Reports = this.filteredItems;
  }
  filterByEmail(value) {
    if (!value) {
      this.getReport();
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.email_Address.indexOf(value) > -1
    );
    this.Reports = this.filteredItems;
  }
  EntryFilterChange(filterByName) {
    if (filterByName == "None") {
      this.showTicketTextbox = false;
      this.showPoolTextbox = false;
      this.showFirstTextbox = false;
      this.showLastTextbox = false;
      this.showEmailTextbox = false;
      this.getReport();
    } else if (filterByName == "Ticket") {
      this.showTicketTextbox = true;
      this.showPoolTextbox = false;
      this.showFirstTextbox = false;
      this.showLastTextbox = false;
      this.showEmailTextbox = false;
    } else if (filterByName == "Pool Name") {
      this.showTicketTextbox = false;
      this.showPoolTextbox = true;
      this.showFirstTextbox = false;
      this.showLastTextbox = false;
      this.showEmailTextbox = false;
    } else if (filterByName == "First Name") {
      this.showTicketTextbox = false;
      this.showPoolTextbox = false;
      this.showFirstTextbox = true;
      this.showLastTextbox = false;
      this.showEmailTextbox = false;
    } else if (filterByName == "Last Name") {
      this.showTicketTextbox = false;
      this.showPoolTextbox = false;
      this.showFirstTextbox = false;
      this.showLastTextbox = true;
      this.showEmailTextbox = false;
    } else if (filterByName == "Email") {
      this.showTicketTextbox = false;
      this.showPoolTextbox = false;
      this.showFirstTextbox = false;
      this.showLastTextbox = false;
      this.showEmailTextbox = true;
    }
  }
}
