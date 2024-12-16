<template>
  <div>
    <h1>档案ID: {{ id }}</h1>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>{{ tableData.name }}档案</span> <!-- 动态显示分类名称 -->
          <el-button type="primary" icon="CirclePlus" round @click="handleDialog(null)">添加</el-button>
          <el-button type="primary" icon="CirclePlus" round @click="edDialog()">导出</el-button>
        </div>
      </template>

      <el-table :data="tableData.list" stripe style="width: 100%">
        <!-- 行号列 -->
        <el-table-column label="序号" width="80">
          <template #default="scope">
            {{ scope.$index + 1 }} <!-- 从1开始计数 -->
          </template>
        </el-table-column>

        <!-- 动态列渲染 -->
        <template v-if="name === '供应商档案'">
          <el-table-column prop="fieldName" label="供应商名称" width="180" />
          <el-table-column prop="supplier_phone" label="供应商电话" width="200" />
          <el-table-column prop="supplier_address" label="供应商地址" width="200" />
        </template>

        <template v-if="name === '客户档案'">
          <el-table-column prop="fieldName" label="客户姓名" width="180" />
          <el-table-column prop="customer_address" label="客户地址" width="200" />
          <el-table-column prop="customer_phone_number" label="客户电话" width="200" />
        </template>

        <template v-if="name === '商品档案'">
          <el-table-column prop="product_code" label="商品编码" width="200" />
          <el-table-column prop="product_type" label="商品类型" width="200" />
          <el-table-column prop="unit_price" label="商品价格" width="200" />
        </template>

        <template v-if="name === '仓库档案'">
          <el-table-column prop="fieldName" label="存货名称" width="200" />
          <el-table-column prop="product_code" label="存货编码" width="200" />
          <el-table-column prop="count" label="数量" width="200" />
          <el-table-column prop="unit_price" label="价单" width="200" />
          <el-table-column prop="total_order" label="总单" width="200" />
        </template>

        

        <!-- <el-table-column prop="fieldName" label="信息" width="180" />
        <el-table-column prop="name" label="显示名称" width="200" />
        <el-table-column prop="fieldValue" label="数值" width="200" /> -->

        <!-- 固定操作列 -->
        <el-table-column fixed="right" label="操作" width="180">
          <template #default="scope">
            <el-button type="success" size="small" @click="handleDialog(scope.row)">修改</el-button>
            <el-button type="danger" size="small" @click="open(scope.row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 传递 id 给 AddUsers 组件 -->
      <AddUsers ref="addCategory" :dialogTitle="dialogTitle" :tableRow="tableRow" :id="id"></AddUsers>
    </el-card>
  </div>
</template>
  
  
  <script setup>
  import { defineProps, reactive, onMounted, ref,  watch ,provide } from 'vue';
  import axios from "@/api/api_config";
  import AddUsers from '@/components/AddDetail.vue';  // 确保路径正确
  import { isNull } from '@/utils/filter';
  import { ElMessage, ElMessageBox } from 'element-plus';
  import Papa from 'papaparse';
  
  const tableData = reactive({ list: [] });
  const addCategory = ref(null); // 获取子组件实例
  const dialogTitle = ref("");
  const tableRow = ref({});
  const name = ref(''); // 添加一个响应式变量来存储 name 值
  
  const props = defineProps({
    id: {
      type: String,
      required: true
    }
  });
  
  // 获取分类的数据
  const getList = () => {
    const categoryId = props.id; // 获取传入的 id
    axios.get(`/CategoryData/${categoryId}`).then((res) => {
      tableData.list = res.data;
      console.log("值",res.data);
      if (res.data.length > 0) {
        name.value = res.data[0].name; // 假设您想要获取数组中第一个对象的 name 属性
      }
     
      console.log("name的值",name.value); // 现在这里会显示 name 属性的值
    }).catch((error) => {
      console.error("Error fetching data: ", error);
      ElMessage.error("数据加载失败");
    });
  };





  provide("getList",getList);

  // 监听 id 的变化，重新加载数据
  watch(() => props.id, (newId) => {
    console.log('Category ID changed:', newId);
    getList(); // id 变化时重新获取数据
  }, { immediate: true }); // immediate: true 让组件初始化时就能调用一次 getList
  

  
  // 打开分类页
  const handleDialog = (row) => {
    if (isNull(row)) {
      dialogTitle.value = "添加";
      tableRow.value = { id: "", fieldName: "" }; // 初始化为空
    } else {
      dialogTitle.value = "修改信息";
      tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
    }
  
    addCategory.value.dialogCategory(); // 调用子组件的方法
  };
  
  const edDialog = async () => {
  try {
    const categoryId = props.id;

    // 调用后端导出接口
    const response = await axios.get(`/CategoryData/${categoryId}`, {
      responseType: "json", // 告诉 Axios 返回 JSON 数据
    });

    // 解析 JSON 数据
    const data = response.data;

    // 将 JSON 数据转换为 CSV 格式
    const csvData = Papa.unparse(data);

    // 创建 Blob 对象
    const blob = new Blob([csvData], { type: "text/csv" });

    // 创建下载链接
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = `Category_${categoryId}_Data.csv`;
    document.body.appendChild(link);

    // 自动点击下载
    link.click();

    // 移除链接
    document.body.removeChild(link);

    ElMessage.success("导出成功！");
  } catch (error) {
    ElMessage.error(`导出失败: ${error.message}`);
  }
};


  // 删除特定数据
  const open = (categoryDataId) => {
    ElMessageBox.confirm(
      '确定要删除这条数据吗?',
      '你确定要删除这条数据吗?',
      {
        confirmButtonText: '确认',
        cancelButtonText: '取消',
        type: 'warning',
      }
    )
      .then(() => {
        const categoryId = props.id;
        axios.delete(`/CategoryData/${categoryId}/${categoryDataId}`).then(() => {
          ElMessage({
            type: 'success',
            message: '删除成功',
          });
          getList(); // 删除后刷新列表
        });
      })
      .catch(() => {
        ElMessage({
          type: 'info',
          message: '取消删除',
        });
      });
  };
  
  onMounted(() => {
    getList();
  });
  </script>
  
  