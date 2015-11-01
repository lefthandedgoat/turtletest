module scripts

//todo combine and minify this stuff
let jquery_1_11_3_min = """<script src="//code.jquery.com/jquery-1.11.3.min.js"></script>"""
let datatable_jquery_1_10_9_min = """<script src="//cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>"""
let datatable_min = """<script src="/js/jquery.datatables.min.js"></script>"""
let datatables_bootstrap_adapter = """<script src="/js/datatables-bootstrap-adapter.js"></script>"""

//todo move this to a file??
let applications_datatable = """
<script type="text/javascript">
  $(document).ready(function(){
    //Basic Instance
    $(".table.table-bordered").dataTable();

    //Search input style
    $('.dataTables_filter input').addClass('form-control').attr('placeholder','Search');
    $('.dataTables_length select').addClass('form-control').attr('size','1');
  });
</script>
"""

let applications_bundle =
  [
    jquery_1_11_3_min
    datatable_jquery_1_10_9_min
    datatable_min
    datatables_bootstrap_adapter
    applications_datatable
  ]
