<template>
    <el-card class="box-card">
  
      <template #header>
        <div class="card-header">
          <span>用户档案</span>
  
          <el-button type="primary" icon="CirclePlus" round @click="handleDialog(null)">添加</el-button>
  
        </div>
      </template>
  
  
      <el-table :data="tableData.list" stripe style="width: 100%">
  <!-- 行号列 -->
  <el-table-column label="序号" width="80">
    <template #default="scope">
      {{ scope.$index + 1 }} <!-- 从1开始计数 -->
    </template>
  </el-table-column>
  
  <el-table-column prop="name" label="名称" width="180" />
  <el-table-column prop="phone" label="手机" width="200" />
  <el-table-column prop="address" label="地址" width="200" />



  <el-table-column fixed="right" label="操作" width="180">
  
  
    
    <template #default="scope">
      <el-button type="success" size="small" @click="handleDialog(scope.row)">
        修改
      </el-button>
      <el-button type="danger" size="small" @click="open(scope.row.id)">
        删除
      </el-button>
    </template>
  </el-table-column>
  
  
  </el-table>
  
  <AddCustomer ref="addCategory"
  :dialogTitle="dialogTitle"
  :tableRow="tableRow"></AddCustomer>
    </el-card>
  
  </template>
  
  
  <script  setup>
  
  
  import { reactive,onMounted,ref,provide } from 'vue';
  import axios from "@/api/api_config";
  import AddCustomer from '@/components/AddCustomer.vue';
  import { isNull } from '@/utils/filter';
  import { ElMessage, ElMessageBox } from 'element-plus'
  
  ///----------------------------------
  const tableData = reactive({list:[]})
  const addCategory = ref(null) //获取子属性组件实例
  const dialogTitle = ref("")
  const tableRow = ref({})
  
  
  //-----------------------------------
  
  
  onMounted(()=>{
  getList()
  })
  
  //获取分类的信息
  const getList=()=>{
  return axios.get('/File_Management/Customer').then((res)=>{
    tableData.list = res.data
    console.log(res.data);
  });
  };
  
  provide("getList",getList);
  
  //打开分类页
  const handleDialog = (row) => {
  if (isNull(row)) {
    dialogTitle.value = "添加";
    tableRow.value = { id: "", name: "" }; // 初始化为空
  } else {
    dialogTitle.value = "修改";
    tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
  }
  
  addCategory.value.dialogCategory(); // 调用子组件的方法
  };
  
  
  
  const open=(id)=>{
  ElMessageBox.confirm(
    '确定要删除吗?',
    '你确定要删除吗',
    {
      confirmButtonText: '确认',
      cancelButtonText: '取消',
      type: 'warning',
    }
  )
    .then(() => {
      axios.delete(`/File_Management/deleteCustomer_Table/${id}`).then(()=>{
        ElMessage({
        type: 'success',
        message: '删除成功',
      });
      getList();
      });
    })
    .catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
  }
  
  </script>